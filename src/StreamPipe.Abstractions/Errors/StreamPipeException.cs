namespace StreamPipe;

/// <summary>
/// Base type for typed StreamPipe public errors (REQ-API-017, REQ-API-018).
/// </summary>
public class StreamPipeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamPipeException"/> class.
    /// </summary>
    /// <param name="category">Stable machine-readable error category.</param>
    /// <param name="message">Human-readable message that must not include secrets or payload bytes.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public StreamPipeException(StreamPipeErrorCategory category, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Category = category;
    }

    /// <summary>
    /// Gets the stable machine-readable error category.
    /// </summary>
    public StreamPipeErrorCategory Category { get; }
}
