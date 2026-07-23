namespace StreamPipe;

/// <summary>
/// Raised when an input exceeds a configured resource limit (REQ-MEM-008, REQ-MEM-009).
/// </summary>
public sealed class ResourceLimitExceededException : StreamPipeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceLimitExceededException"/> class.
    /// </summary>
    /// <param name="limitName">Observable limit name for diagnostics (REQ-MEM-010).</param>
    /// <param name="limitValue">Configured limit value.</param>
    /// <param name="message">Human-readable message without payload contents.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public ResourceLimitExceededException(
        string limitName,
        long limitValue,
        string message,
        Exception? innerException = null)
        : base(StreamPipeErrorCategory.ResourceLimit, message, innerException)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(limitName);
        LimitName = limitName;
        LimitValue = limitValue;
    }

    /// <summary>
    /// Gets the observable name of the exceeded limit.
    /// </summary>
    public string LimitName { get; }

    /// <summary>
    /// Gets the configured limit value.
    /// </summary>
    public long LimitValue { get; }
}
