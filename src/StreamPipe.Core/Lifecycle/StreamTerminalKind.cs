namespace StreamPipe.Core;

/// <summary>
/// Terminal outcomes that a data stream or sink may reach (REQ-DATA-018).
/// </summary>
public enum StreamTerminalKind
{
    /// <summary>No terminal transition has occurred.</summary>
    None = 0,

    /// <summary>Successful completion.</summary>
    Completed = 1,

    /// <summary>Failed with an error.</summary>
    Failed = 2,

    /// <summary>Cancelled by the caller or upstream cancellation.</summary>
    Cancelled = 3,

    /// <summary>Disposed; local resources released.</summary>
    Disposed = 4,
}
