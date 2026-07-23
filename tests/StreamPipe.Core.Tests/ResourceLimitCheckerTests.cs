using StreamPipe;
using StreamPipe.Core;

namespace StreamPipe.Core.Tests;

public class ResourceLimitCheckerTests
{
    private static ResourceLimitChecker CreateChecker() =>
        new(new StreamResourceLimits(
            maxFramePayloadBytes: 32,
            maxSchemaMetadataBytes: 16,
            maxNestingDepth: 2,
            maxBatchRecordCount: 3,
            maxBatchByteCount: 64,
            maxInFlightBytes: 128,
            maxQueuedApplicationWorkItems: 2));

    // REQ-MEM-008 — reject input that exceeds a resource limit before unbounded allocation.
    [Fact]
    public void EnsureFramePayload_REQ_MEM_008_Rejects_Oversize_Payload()
    {
        var checker = CreateChecker();
        var ex = Assert.Throws<ResourceLimitExceededException>(() => checker.EnsureFramePayloadWithinLimit(33));
        Assert.Equal(nameof(StreamResourceLimits.MaxFramePayloadBytes), ex.LimitName);
    }

    // REQ-MEM-009 — limit failure MUST be distinguishable from successful completion.
    [Fact]
    public void LimitFailure_REQ_MEM_009_Uses_ResourceLimit_Category()
    {
        var checker = CreateChecker();
        var ex = Assert.Throws<ResourceLimitExceededException>(() => checker.EnsureBatchRecordCountWithinLimit(4));
        Assert.Equal(StreamPipeErrorCategory.ResourceLimit, ex.Category);
    }

    // REQ-MEM-010 — configured limits are observable in diagnostics without payload values.
    [Fact]
    public void Limits_REQ_MEM_010_Are_Observable_On_Checker_And_Exception()
    {
        var checker = CreateChecker();
        Assert.Equal(3, checker.Limits.MaxBatchRecordCount);

        var ex = Assert.Throws<ResourceLimitExceededException>(() => checker.EnsureBatchRecordCountWithinLimit(10));
        Assert.Equal(3, ex.LimitValue);
        Assert.DoesNotContain("secret", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    // REQ-MEM-008 — nesting depth limit applied before deep traversal.
    [Fact]
    public void Nesting_REQ_MEM_008_Rejects_Deep_Logical_Types()
    {
        var checker = CreateChecker();
        var deep = LogicalType.List(LogicalType.List(LogicalType.Int32, false), false);
        Assert.Throws<ResourceLimitExceededException>(() => checker.EnsureLogicalTypeWithinNestingLimit(deep));
    }
}
