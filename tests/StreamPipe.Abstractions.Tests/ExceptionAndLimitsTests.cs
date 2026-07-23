using StreamPipe;

namespace StreamPipe.Abstractions.Tests;

public class ExceptionAndLimitsTests
{
    // REQ-API-017 — public errors expose a stable category and human-readable message.
    [Fact]
    public void StreamPipeException_REQ_API_017_Exposes_Category_And_Message()
    {
        var ex = new StreamStateException("invalid state");
        Assert.Equal(StreamPipeErrorCategory.State, ex.Category);
        Assert.Equal("invalid state", ex.Message);
    }

    // REQ-API-018 — public errors must not require payload/secret fields.
    [Fact]
    public void ResourceLimitExceeded_REQ_API_018_Exposes_Limit_Diagnostics_Only()
    {
        var ex = new ResourceLimitExceededException(
            nameof(StreamResourceLimits.MaxBatchRecordCount),
            10,
            "batch too large");

        Assert.Equal(StreamPipeErrorCategory.ResourceLimit, ex.Category);
        Assert.Equal(nameof(StreamResourceLimits.MaxBatchRecordCount), ex.LimitName);
        Assert.Equal(10, ex.LimitValue);
        Assert.DoesNotContain("payload", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    // REQ-MEM-002 — unbounded input dimensions have configured upper limits.
    [Fact]
    public void StreamResourceLimits_REQ_MEM_002_Require_Positive_Configured_Limits()
    {
        var limits = new StreamResourceLimits(
            maxFramePayloadBytes: 1024,
            maxSchemaMetadataBytes: 256,
            maxNestingDepth: 8,
            maxBatchRecordCount: 100,
            maxBatchByteCount: 4096,
            maxInFlightBytes: 8192,
            maxQueuedApplicationWorkItems: 16);

        Assert.Equal(1024, limits.MaxFramePayloadBytes);
        Assert.Throws<ArgumentOutOfRangeException>(() => new StreamResourceLimits(
            maxFramePayloadBytes: 0,
            maxSchemaMetadataBytes: 256,
            maxNestingDepth: 8,
            maxBatchRecordCount: 100,
            maxBatchByteCount: 4096,
            maxInFlightBytes: 8192,
            maxQueuedApplicationWorkItems: 16));
    }
}
