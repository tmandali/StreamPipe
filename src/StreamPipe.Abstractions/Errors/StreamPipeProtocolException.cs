namespace StreamPipe;

/// <summary>
/// Reserved for protocol-layer failures once SPSS-100 defines wire behavior.
/// </summary>
public sealed class StreamPipeProtocolException : StreamPipeException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamPipeProtocolException"/> class.
    /// </summary>
    /// <param name="message">Human-readable message.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public StreamPipeProtocolException(string message, Exception? innerException = null)
        : base(StreamPipeErrorCategory.Protocol, message, innerException)
    {
    }
}
