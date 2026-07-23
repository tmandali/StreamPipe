using StreamPipe;

namespace StreamPipe.Abstractions.Tests;

public class ColumnValueTests
{
    // REQ-API-007 — null is represented separately from empty or default values.
    [Fact]
    public void Null_REQ_API_007_Is_Distinct_From_Empty_And_Zero()
    {
        Assert.True(ColumnValue.Null.IsNull);
        Assert.False(ColumnValue.FromUtf8(string.Empty).IsNull);
        Assert.False(ColumnValue.FromBinary(ReadOnlyMemory<byte>.Empty).IsNull);
        Assert.False(ColumnValue.FromInt32(0).IsNull);
        Assert.False(ColumnValue.FromBoolean(false).IsNull);
    }

    // REQ-DATA-006 — non-nullable semantics are enforced at validation time; null value itself is explicit.
    [Fact]
    public void Null_REQ_DATA_006_Is_Explicit_Logical_Null()
    {
        Assert.False(ColumnValue.Null.TryGetRawValue(out _));
    }
}
