namespace StreamPipe;

/// <summary>
/// Push-based data sink used by upload and bulk-ingest adapters (REQ-API-014 through REQ-API-016).
/// </summary>
public interface IDataSink : IAsyncDisposable
{
    /// <summary>
    /// Gets the stable sink schema.
    /// </summary>
    DataSchema Schema { get; }

    /// <summary>
    /// Writes one record in schema field order.
    /// Implementations must validate record length, field order, nullability, and logical type before accepting the write (REQ-API-014).
    /// </summary>
    /// <param name="record">Record values aligned to <see cref="Schema"/> fields.</param>
    /// <param name="cancellationToken">Caller-supplied cancellation token (REQ-API-004).</param>
    /// <returns>A value task that completes when the record has been accepted.</returns>
    ValueTask WriteAsync(ReadOnlyMemory<ColumnValue> record, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the sink as successfully completed. Idempotent after successful completion (REQ-API-016).
    /// </summary>
    /// <param name="cancellationToken">Caller-supplied cancellation token.</param>
    /// <returns>A value task that completes when completion has been accepted.</returns>
    ValueTask CompleteAsync(CancellationToken cancellationToken = default);
}
