namespace StreamPipe.Core;

/// <summary>
/// Protocol-independent lifecycle guard for <see cref="IDataSink"/> implementations
/// (REQ-API-015, REQ-API-016, REQ-DATA-018).
/// </summary>
public sealed class DataSinkLifecycle
{
    private StreamTerminalKind _terminal;
    private Exception? _failure;

    /// <summary>
    /// Gets the terminal kind, or <see cref="StreamTerminalKind.None"/> while active.
    /// </summary>
    public StreamTerminalKind Terminal => _terminal;

    /// <summary>
    /// Ensures writes are still accepted (REQ-API-015).
    /// </summary>
    /// <param name="cancellationToken">Caller cancellation token.</param>
    public void EnsureWritable(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfNotWritable();
    }

    /// <summary>
    /// Marks successful completion. Subsequent calls are idempotent (REQ-API-016).
    /// </summary>
    public void CompleteSuccessfully()
    {
        switch (_terminal)
        {
            case StreamTerminalKind.None:
                _terminal = StreamTerminalKind.Completed;
                return;
            case StreamTerminalKind.Completed:
                return;
            case StreamTerminalKind.Failed:
                throw new StreamStateException("Cannot complete a sink that has already failed.", _failure);
            case StreamTerminalKind.Cancelled:
                throw new StreamStateException("Cannot complete a sink that has already been cancelled.");
            case StreamTerminalKind.Disposed:
                throw new ObjectDisposedException(nameof(IDataSink));
            default:
                throw new StreamStateException($"Unknown sink terminal kind '{_terminal}'.");
        }
    }

    /// <summary>
    /// Marks failure. Writes after failure are rejected (REQ-API-015).
    /// </summary>
    /// <param name="error">Failure exception.</param>
    public void Fail(Exception error)
    {
        ArgumentNullException.ThrowIfNull(error);
        if (_terminal is StreamTerminalKind.Disposed or StreamTerminalKind.Completed)
        {
            return;
        }

        _failure = error;
        _terminal = StreamTerminalKind.Failed;
    }

    /// <summary>
    /// Marks cancellation. Writes after cancellation are rejected (REQ-API-015).
    /// </summary>
    public void Cancel()
    {
        if (_terminal is StreamTerminalKind.Disposed or StreamTerminalKind.Completed or StreamTerminalKind.Failed)
        {
            return;
        }

        _terminal = StreamTerminalKind.Cancelled;
    }

    /// <summary>
    /// Marks disposal.
    /// </summary>
    public void Dispose()
    {
        _terminal = StreamTerminalKind.Disposed;
    }

    private void ThrowIfNotWritable()
    {
        switch (_terminal)
        {
            case StreamTerminalKind.None:
                return;
            case StreamTerminalKind.Completed:
                throw new StreamStateException("Cannot write to a sink after successful completion.");
            case StreamTerminalKind.Failed:
                throw new StreamStateException("Cannot write to a sink after failure.", _failure);
            case StreamTerminalKind.Cancelled:
                throw new StreamStateException("Cannot write to a sink after cancellation.");
            case StreamTerminalKind.Disposed:
                throw new ObjectDisposedException(nameof(IDataSink));
            default:
                throw new StreamStateException($"Unknown sink terminal kind '{_terminal}'.");
        }
    }
}
