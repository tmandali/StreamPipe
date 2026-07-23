using System.Text;

namespace StreamPipe.Core;

/// <summary>
/// Enforces configured resource limits before unbounded allocation (REQ-MEM-008, REQ-MEM-009, REQ-MEM-010).
/// </summary>
public sealed class ResourceLimitChecker
{
    private readonly StreamResourceLimits _limits;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceLimitChecker"/> class.
    /// </summary>
    /// <param name="limits">Configured limits.</param>
    public ResourceLimitChecker(StreamResourceLimits limits)
    {
        _limits = limits ?? throw new ArgumentNullException(nameof(limits));
    }

    /// <summary>
    /// Gets the configured limits for diagnostics (REQ-MEM-010).
    /// </summary>
    public StreamResourceLimits Limits => _limits;

    /// <summary>
    /// Rejects a frame payload that exceeds <see cref="StreamResourceLimits.MaxFramePayloadBytes"/>.
    /// </summary>
    /// <param name="payloadByteCount">Observed payload size.</param>
    public void EnsureFramePayloadWithinLimit(long payloadByteCount)
    {
        if (payloadByteCount > _limits.MaxFramePayloadBytes)
        {
            throw new ResourceLimitExceededException(
                nameof(StreamResourceLimits.MaxFramePayloadBytes),
                _limits.MaxFramePayloadBytes,
                $"Frame payload size {payloadByteCount} exceeds limit {_limits.MaxFramePayloadBytes}.");
        }
    }

    /// <summary>
    /// Rejects schema metadata that exceeds <see cref="StreamResourceLimits.MaxSchemaMetadataBytes"/>.
    /// Size is measured as UTF-8 byte length of keys and values.
    /// </summary>
    /// <param name="metadata">Schema or field metadata.</param>
    public void EnsureSchemaMetadataWithinLimit(SchemaMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        long total = 0;
        foreach (var pair in metadata.Entries)
        {
            total += Encoding.UTF8.GetByteCount(pair.Key);
            total += Encoding.UTF8.GetByteCount(pair.Value);
            if (total > _limits.MaxSchemaMetadataBytes)
            {
                throw new ResourceLimitExceededException(
                    nameof(StreamResourceLimits.MaxSchemaMetadataBytes),
                    _limits.MaxSchemaMetadataBytes,
                    $"Schema metadata size exceeds limit {_limits.MaxSchemaMetadataBytes}.");
            }
        }
    }

    /// <summary>
    /// Rejects nesting deeper than <see cref="StreamResourceLimits.MaxNestingDepth"/>.
    /// </summary>
    /// <param name="depth">Observed nesting depth (1 = top-level field type).</param>
    public void EnsureNestingDepthWithinLimit(int depth)
    {
        if (depth > _limits.MaxNestingDepth)
        {
            throw new ResourceLimitExceededException(
                nameof(StreamResourceLimits.MaxNestingDepth),
                _limits.MaxNestingDepth,
                $"Nesting depth {depth} exceeds limit {_limits.MaxNestingDepth}.");
        }
    }

    /// <summary>
    /// Rejects a batch whose record count exceeds <see cref="StreamResourceLimits.MaxBatchRecordCount"/>.
    /// </summary>
    /// <param name="recordCount">Observed batch record count.</param>
    public void EnsureBatchRecordCountWithinLimit(int recordCount)
    {
        if (recordCount > _limits.MaxBatchRecordCount)
        {
            throw new ResourceLimitExceededException(
                nameof(StreamResourceLimits.MaxBatchRecordCount),
                _limits.MaxBatchRecordCount,
                $"Batch record count {recordCount} exceeds limit {_limits.MaxBatchRecordCount}.");
        }
    }

    /// <summary>
    /// Rejects a batch whose byte size exceeds <see cref="StreamResourceLimits.MaxBatchByteCount"/>.
    /// </summary>
    /// <param name="byteCount">Observed batch byte count.</param>
    public void EnsureBatchByteCountWithinLimit(long byteCount)
    {
        if (byteCount > _limits.MaxBatchByteCount)
        {
            throw new ResourceLimitExceededException(
                nameof(StreamResourceLimits.MaxBatchByteCount),
                _limits.MaxBatchByteCount,
                $"Batch byte count {byteCount} exceeds limit {_limits.MaxBatchByteCount}.");
        }
    }

    /// <summary>
    /// Rejects in-flight bytes that exceed <see cref="StreamResourceLimits.MaxInFlightBytes"/>.
    /// </summary>
    /// <param name="inFlightBytes">Observed in-flight byte count.</param>
    public void EnsureInFlightBytesWithinLimit(long inFlightBytes)
    {
        if (inFlightBytes > _limits.MaxInFlightBytes)
        {
            throw new ResourceLimitExceededException(
                nameof(StreamResourceLimits.MaxInFlightBytes),
                _limits.MaxInFlightBytes,
                $"In-flight byte count {inFlightBytes} exceeds limit {_limits.MaxInFlightBytes}.");
        }
    }

    /// <summary>
    /// Rejects queued application work that exceeds <see cref="StreamResourceLimits.MaxQueuedApplicationWorkItems"/>.
    /// </summary>
    /// <param name="queuedItems">Observed queued work item count.</param>
    public void EnsureQueuedApplicationWorkWithinLimit(int queuedItems)
    {
        if (queuedItems > _limits.MaxQueuedApplicationWorkItems)
        {
            throw new ResourceLimitExceededException(
                nameof(StreamResourceLimits.MaxQueuedApplicationWorkItems),
                _limits.MaxQueuedApplicationWorkItems,
                $"Queued application work {queuedItems} exceeds limit {_limits.MaxQueuedApplicationWorkItems}.");
        }
    }

    /// <summary>
    /// Walks a logical type tree and enforces nesting depth before recursive traversal allocates unbounded memory.
    /// </summary>
    /// <param name="type">Root logical type.</param>
    public void EnsureLogicalTypeWithinNestingLimit(LogicalType type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Walk(type, depth: 1);
    }

    private void Walk(LogicalType type, int depth)
    {
        EnsureNestingDepthWithinLimit(depth);

        switch (type)
        {
            case LogicalType.ListLogicalType list:
                Walk(list.ElementType, depth + 1);
                break;
            case LogicalType.StructLogicalType structType:
                foreach (var field in structType.Fields)
                {
                    Walk(field.Type, depth + 1);
                }

                break;
            case LogicalType.MapLogicalType map:
                Walk(map.KeyType, depth + 1);
                Walk(map.ValueType, depth + 1);
                break;
            default:
                break;
        }
    }
}
