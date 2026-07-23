namespace StreamPipe.Core;

/// <summary>
/// Bounded <see cref="IDataStream"/> over a fixed, finite set of records.
/// Retained memory is proportional to the supplied record set, not to an unbounded producer (REQ-MEM-001, REQ-MEM-013).
/// </summary>
public sealed class FixedRecordDataReader : IDataStream
{
    private readonly ColumnValue[][] _records;
    private readonly DataStreamLifecycle _lifecycle = new();
    private int _index = -1;

    /// <summary>
    /// Initializes a new instance of the <see cref="FixedRecordDataReader"/> class.
    /// </summary>
    /// <param name="schema">Stream schema, available before the first read (REQ-DATA-016).</param>
    /// <param name="records">Finite record set; each record is validated against <paramref name="schema"/>.</param>
    public FixedRecordDataReader(DataSchema schema, IReadOnlyList<ColumnValue[]> records)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        ArgumentNullException.ThrowIfNull(records);

        _records = new ColumnValue[records.Count][];
        for (var i = 0; i < records.Count; i++)
        {
            var record = records[i] ?? throw new ArgumentNullException(nameof(records));
            RecordValidator.Validate(schema, record);
            _records[i] = record;
        }
    }

    /// <inheritdoc />
    public DataSchema Schema { get; }

    /// <inheritdoc />
    public ReadOnlySpan<ColumnValue> Current
    {
        get
        {
            _lifecycle.EnsureCurrentValid();
            return _records[_index];
        }
    }

    /// <inheritdoc />
    public ValueTask<bool> ReadAsync(CancellationToken cancellationToken = default)
    {
        _lifecycle.BeginRead(cancellationToken);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var next = _index + 1;
            if (next >= _records.Length)
            {
                _lifecycle.CompleteSuccessfully();
                return ValueTask.FromResult(false);
            }

            _index = next;
            _lifecycle.OnRecordAvailable();
            return ValueTask.FromResult(true);
        }
        catch (OperationCanceledException)
        {
            _lifecycle.Cancel();
            throw;
        }
        catch (Exception ex) when (ex is not StreamStateException and not ObjectDisposedException)
        {
            _lifecycle.Fail(ex);
            throw;
        }
        finally
        {
            _lifecycle.EndRead();
        }
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        _lifecycle.Dispose();
        return ValueTask.CompletedTask;
    }
}
