namespace StreamPipe;

/// <summary>
/// Machine-readable category for a public StreamPipe error (REQ-API-017).
/// </summary>
public enum StreamPipeErrorCategory
{
    /// <summary>Protocol session or frame semantics failure.</summary>
    Protocol = 1,

    /// <summary>Transport I/O or channel failure.</summary>
    Transport = 2,

    /// <summary>Payload format or logical-type representation failure.</summary>
    Format = 3,

    /// <summary>Configured or negotiated resource limit exceeded.</summary>
    ResourceLimit = 4,

    /// <summary>Invalid lifecycle or operation state transition.</summary>
    State = 5,
}
