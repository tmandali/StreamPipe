namespace StreamPipe;

/// <summary>
/// Reserved for transport-adapter failures once a transport profile is specified.
/// </summary>
public sealed class StreamPipeTransportException : StreamPipeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamPipeTransportException"/> class.
    /// </summary>
    /// <param name="message">Human-readable message.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public StreamPipeTransportException(string message, Exception? innerException = null)
        : base(StreamPipeErrorCategory.Transport, message, innerException)
    {
    }
}
