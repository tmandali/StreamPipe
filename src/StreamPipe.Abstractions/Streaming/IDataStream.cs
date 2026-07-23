namespace StreamPipe;

/// <summary>
/// Row-oriented, pull-based logical stream contract (REQ-API-009 through REQ-API-013).
/// </summary>
public interface IDataStream : IAsyncDisposable
{
    /// <summary>
    /// Gets the stream schema. Available before the first record is read (REQ-API-009, REQ-DATA-016).
    /// </summary>
    DataSchema Schema { get; }

    /// <summary>
    /// Advances to the next record.
    /// Returns <see langword="true"/> when <see cref="Current"/> contains one record.
    /// Returns <see langword="false"/> only after successful end-of-stream.
    /// Cancellation, failure, invalid data, and resource-limit violations must not return <see langword="false"/> (REQ-API-011).
    /// </summary>
    /// <param name="cancellationToken">Caller-supplied cancellation token (REQ-API-004).</param>
    /// <returns>A value task that completes with the read result.</returns>
    ValueTask<bool> ReadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current record. Valid only after a successful <see cref="ReadAsync"/> and until the next read,
    /// disposal, or terminal transition (REQ-API-010).
    /// </summary>
    ReadOnlySpan<ColumnValue> Current { get; }
}
