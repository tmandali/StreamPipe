namespace StreamPipe;

/// <summary>
/// Raised when a stream or sink operation violates lifecycle rules (REQ-API-015, REQ-DATA-017).
/// </summary>
public sealed class StreamStateException : StreamPipeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamStateException"/> class.
    /// </summary>
    /// <param name="message">Human-readable message.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public StreamStateException(string message, Exception? innerException = null)
        : base(StreamPipeErrorCategory.State, message, innerException)
    {
    }
}
