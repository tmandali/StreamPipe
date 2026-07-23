using StreamPipe;

namespace StreamPipe.Abstractions.Tests;

public class LogicalTypeTests
{
    // REQ-TYPE-001 — A type declaration MUST be immutable (factory returns stable instances / frozen parameters).
    [Fact]
    public void Singletons_REQ_TYPE_001_Are_Stable_References()
    {
        Assert.Same(LogicalType.Boolean, LogicalType.Boolean);
        Assert.Same(LogicalType.Int32, LogicalType.Int32);
    }

    // REQ-TYPE-007 / REQ-TYPE-008 — Decimal precision and scale bounds.
    [Fact]
    public void Decimal_REQ_TYPE_007_008_Enforces_Precision_And_Scale()
    {
        var type = Assert.IsType<LogicalType.DecimalLogicalType>(LogicalType.Decimal(38, 10));
        Assert.Equal(38, type.Precision);
        Assert.Equal(10, type.Scale);

        Assert.Throws<ArgumentOutOfRangeException>(() => LogicalType.Decimal(0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => LogicalType.Decimal(10, 11));
    }

    // REQ-DATA-010 / REQ-TYPE-011 — temporal parameters preserved.
    [Fact]
    public void Timestamp_REQ_DATA_010_Preserves_Unit_And_Timezone()
    {
        var type = Assert.IsType<LogicalType.TimestampLogicalType>(
            LogicalType.Timestamp(TemporalUnit.Microsecond, "Europe/Istanbul"));
        Assert.Equal(TemporalUnit.Microsecond, type.Unit);
        Assert.Equal("Europe/Istanbul", type.TimeZone);
    }

    // REQ-DATA-010 — nested-type parameters preserved.
    [Fact]
    public void NestedTypes_REQ_DATA_010_Preserve_Parameters()
    {
        var list = Assert.IsType<LogicalType.ListLogicalType>(LogicalType.List(LogicalType.Int32, elementNullable: true));
        Assert.Same(LogicalType.Int32, list.ElementType);
        Assert.True(list.ElementNullable);

        var map = Assert.IsType<LogicalType.MapLogicalType>(
            LogicalType.Map(LogicalType.Utf8, LogicalType.Boolean, valueNullable: false));
        Assert.Same(LogicalType.Utf8, map.KeyType);
        Assert.Same(LogicalType.Boolean, map.ValueType);
        Assert.False(map.ValueNullable);
    }
}
