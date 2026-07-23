namespace StreamPipe;

/// <summary>
/// Raised when schema or value data cannot be represented faithfully (REQ-DATA-009, REQ-API-014).
/// </summary>
public sealed class StreamPipeFormatException : StreamPipeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamPipeFormatException"/> class.
    /// </summary>
    /// <param name="message">Human-readable message without payload bytes.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public StreamPipeFormatException(string message, Exception? innerException = null)
        : base(StreamPipeErrorCategory.Format, message, innerException)
    {
    }
}
