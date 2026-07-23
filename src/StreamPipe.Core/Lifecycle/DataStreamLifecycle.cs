namespace StreamPipe.Core;

/// <summary>
/// Protocol-independent lifecycle guard for <see cref="IDataStream"/> implementations
/// (REQ-API-010, REQ-API-011, REQ-API-012, REQ-API-013, REQ-DATA-017, REQ-DATA-018).
/// </summary>
public sealed class DataStreamLifecycle
{
    private int _readGate;
    private bool _hasCurrent;
    private StreamTerminalKind _terminal;
    private Exception? _failure;

    /// <summary>
    /// Gets the terminal kind, or <see cref="StreamTerminalKind.None"/> while active.
    /// </summary>
    public StreamTerminalKind Terminal => _terminal;

    /// <summary>
    /// Gets a value indicating whether <see cref="IDataStream.Current"/> is valid.
    /// </summary>
    public bool HasCurrent => _hasCurrent && _terminal == StreamTerminalKind.None;

    /// <summary>
    /// Begins a read operation, enforcing single-reader semantics (REQ-API-012).
    /// </summary>
    /// <param name="cancellationToken">Caller cancellation token.</param>
    public void BeginRead(CancellationToken cancellationToken)
    {
        ThrowIfTerminal();
        cancellationToken.ThrowIfCancellationRequested();

        if (Interlocked.CompareExchange(ref _readGate, 1, 0) != 0)
        {
            throw new StreamStateException("Concurrent ReadAsync calls on the same stream are not allowed.");
        }
    }

    /// <summary>
    /// Ends a read operation started by <see cref="BeginRead"/>.
    /// </summary>
    public void EndRead()
    {
        Interlocked.Exchange(ref _readGate, 0);
    }

    /// <summary>
    /// Marks a successful record read so that <see cref="HasCurrent"/> becomes true (REQ-API-010).
    /// </summary>
    public void OnRecordAvailable()
    {
        ThrowIfTerminal();
        _hasCurrent = true;
    }

    /// <summary>
    /// Marks successful end-of-stream. Subsequent reads must not deliver records (REQ-DATA-017).
    /// </summary>
    public void CompleteSuccessfully()
    {
        ThrowIfTerminal();
        _hasCurrent = false;
        _terminal = StreamTerminalKind.Completed;
    }

    /// <summary>
    /// Marks failure. Must not be reported as a <see langword="false"/> read result (REQ-API-011).
    /// </summary>
    /// <param name="error">Failure exception.</param>
    public void Fail(Exception error)
    {
        ArgumentNullException.ThrowIfNull(error);
        if (_terminal is StreamTerminalKind.Disposed)
        {
            return;
        }

        _hasCurrent = false;
        _failure = error;
        _terminal = StreamTerminalKind.Failed;
    }

    /// <summary>
    /// Marks cancellation. Must not be reported as a <see langword="false"/> read result (REQ-API-011).
    /// </summary>
    public void Cancel()
    {
        if (_terminal is StreamTerminalKind.Disposed or StreamTerminalKind.Completed or StreamTerminalKind.Failed)
        {
            return;
        }

        _hasCurrent = false;
        _terminal = StreamTerminalKind.Cancelled;
    }

    /// <summary>
    /// Marks disposal and clears current (REQ-API-013).
    /// </summary>
    public void Dispose()
    {
        _hasCurrent = false;
        _terminal = StreamTerminalKind.Disposed;
        Interlocked.Exchange(ref _readGate, 0);
    }

    /// <summary>
    /// Ensures current is valid before exposing <see cref="IDataStream.Current"/>.
    /// </summary>
    public void EnsureCurrentValid()
    {
        if (!HasCurrent)
        {
            throw new StreamStateException(
                "IDataStream.Current is valid only after a successful ReadAsync and until the next read, disposal, or terminal transition.");
        }
    }

    /// <summary>
    /// Throws when the stream is in a terminal state, preserving failure vs cancellation vs completion (REQ-DATA-018).
    /// </summary>
    public void ThrowIfTerminal()
    {
        switch (_terminal)
        {
            case StreamTerminalKind.None:
                return;
            case StreamTerminalKind.Completed:
                throw new StreamStateException("The data stream has already completed successfully.");
            case StreamTerminalKind.Failed:
                throw new StreamStateException("The data stream has failed.", _failure);
            case StreamTerminalKind.Cancelled:
                throw new OperationCanceledException("The data stream was cancelled.");
            case StreamTerminalKind.Disposed:
                throw new ObjectDisposedException(nameof(IDataStream));
            default:
                throw new StreamStateException($"Unknown stream terminal kind '{_terminal}'.");
        }
    }
}
