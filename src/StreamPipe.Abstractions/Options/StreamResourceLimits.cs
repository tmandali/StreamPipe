namespace StreamPipe;

/// <summary>
/// Configured upper bounds for stream-data retention and processing (REQ-MEM-002, REQ-MEM-008).
/// SPSS-030 does not prescribe numeric defaults; callers must supply explicit positive limits.
/// </summary>
public sealed class StreamResourceLimits
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamResourceLimits"/> class.
    /// </summary>
    /// <param name="maxFramePayloadBytes">Maximum frame payload size in bytes.</param>
    /// <param name="maxSchemaMetadataBytes">Maximum schema metadata size in bytes.</param>
    /// <param name="maxNestingDepth">Maximum nested-type depth.</param>
    /// <param name="maxBatchRecordCount">Maximum records per batch.</param>
    /// <param name="maxBatchByteCount">Maximum bytes per batch.</param>
    /// <param name="maxInFlightBytes">Maximum in-flight bytes.</param>
    /// <param name="maxQueuedApplicationWorkItems">Maximum queued application work items.</param>
    public StreamResourceLimits(
        int maxFramePayloadBytes,
        int maxSchemaMetadataBytes,
        int maxNestingDepth,
        int maxBatchRecordCount,
        long maxBatchByteCount,
        long maxInFlightBytes,
        int maxQueuedApplicationWorkItems)
    {
        MaxFramePayloadBytes = RequirePositive(maxFramePayloadBytes, nameof(maxFramePayloadBytes));
        MaxSchemaMetadataBytes = RequirePositive(maxSchemaMetadataBytes, nameof(maxSchemaMetadataBytes));
        MaxNestingDepth = RequirePositive(maxNestingDepth, nameof(maxNestingDepth));
        MaxBatchRecordCount = RequirePositive(maxBatchRecordCount, nameof(maxBatchRecordCount));
        MaxBatchByteCount = RequirePositive(maxBatchByteCount, nameof(maxBatchByteCount));
        MaxInFlightBytes = RequirePositive(maxInFlightBytes, nameof(maxInFlightBytes));
        MaxQueuedApplicationWorkItems = RequirePositive(maxQueuedApplicationWorkItems, nameof(maxQueuedApplicationWorkItems));
    }

    /// <summary>Gets the maximum frame payload size in bytes.</summary>
    public int MaxFramePayloadBytes { get; }

    /// <summary>Gets the maximum schema metadata size in bytes.</summary>
    public int MaxSchemaMetadataBytes { get; }

    /// <summary>Gets the maximum nested-type depth.</summary>
    public int MaxNestingDepth { get; }

    /// <summary>Gets the maximum records per batch.</summary>
    public int MaxBatchRecordCount { get; }

    /// <summary>Gets the maximum bytes per batch.</summary>
    public long MaxBatchByteCount { get; }

    /// <summary>Gets the maximum in-flight bytes.</summary>
    public long MaxInFlightBytes { get; }

    /// <summary>Gets the maximum queued application work items.</summary>
    public int MaxQueuedApplicationWorkItems { get; }

    private static int RequirePositive(int value, string name)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(name, value, "Resource limits must be positive.");
        }

        return value;
    }

    private static long RequirePositive(long value, string name)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(name, value, "Resource limits must be positive.");
        }

        return value;
    }
}
