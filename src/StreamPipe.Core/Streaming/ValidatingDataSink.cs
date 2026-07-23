namespace StreamPipe.Core;

/// <summary>
/// <see cref="IDataSink"/> that validates each record and forwards accepted writes to a callback (REQ-API-014).
/// Does not accumulate an unbounded full-stream collection (REQ-MEM-013).
/// </summary>
public sealed class ValidatingDataSink : IDataSink
{
    private readonly Func<ReadOnlyMemory<ColumnValue>, CancellationToken, ValueTask> _onWrite;
    private readonly Func<CancellationToken, ValueTask>? _onComplete;
    private readonly DataSinkLifecycle _lifecycle = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatingDataSink"/> class.
    /// </summary>
    /// <param name="schema">Stable sink schema.</param>
    /// <param name="onWrite">Callback invoked after validation for each accepted record.</param>
    /// <param name="onComplete">Optional completion callback.</param>
    public ValidatingDataSink(
        DataSchema schema,
        Func<ReadOnlyMemory<ColumnValue>, CancellationToken, ValueTask> onWrite,
        Func<CancellationToken, ValueTask>? onComplete = null)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        _onWrite = onWrite ?? throw new ArgumentNullException(nameof(onWrite));
        _onComplete = onComplete;
    }

    /// <inheritdoc />
    public DataSchema Schema { get; }

    /// <inheritdoc />
    public async ValueTask WriteAsync(ReadOnlyMemory<ColumnValue> record, CancellationToken cancellationToken = default)
    {
        _lifecycle.EnsureWritable(cancellationToken);

        try
        {
            RecordValidator.Validate(Schema, record.Span);
            await _onWrite(record, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            _lifecycle.Cancel();
            throw;
        }
        catch (Exception ex) when (ex is not StreamStateException and not ObjectDisposedException and not StreamPipeFormatException)
        {
            _lifecycle.Fail(ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask CompleteAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var alreadyCompleted = _lifecycle.Terminal == StreamTerminalKind.Completed;
        _lifecycle.CompleteSuccessfully();

        if (alreadyCompleted)
        {
            return;
        }

        if (_onComplete is not null)
        {
            await _onComplete(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        _lifecycle.Dispose();
        return ValueTask.CompletedTask;
    }
}
