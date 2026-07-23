using StreamPipe;

namespace StreamPipe.Abstractions.Tests;

public class ColumnValueTests
{
    // REQ-API-007 / REQ-TYPE-017 — null is distinct from empty or default values.
    [Fact]
    public void Null_REQ_API_007_Is_Distinct_From_Empty_And_Zero()
    {
        Assert.True(ColumnValue.Null.IsNull);
        Assert.Null(ColumnValue.Null.Type);
        Assert.False(ColumnValue.FromUtf8(string.Empty).IsNull);
        Assert.False(ColumnValue.FromBinary(ReadOnlyMemory<byte>.Empty).IsNull);
        Assert.False(ColumnValue.FromInt32(0).IsNull);
        Assert.False(ColumnValue.FromBoolean(false).IsNull);
    }

    // REQ-TYPE-020 — non-null values retain declared logical type.
    [Fact]
    public void NonNull_REQ_TYPE_020_Retains_LogicalType()
    {
        var value = ColumnValue.FromInt32(42);
        Assert.Same(LogicalType.Int32, value.Type);
        Assert.Equal(42, value.GetInt32());

        var decimalType = Assert.IsType<LogicalType.DecimalLogicalType>(LogicalType.Decimal(10, 2));
        var decimalValue = ColumnValue.FromDecimal(decimalType, 12.34m);
        Assert.Same(decimalType, decimalValue.Type);
        Assert.Equal(12.34m, decimalValue.GetDecimal());
    }

    // REQ-TYPE-011 — timestamp unit/timezone retained on the value's type.
    [Fact]
    public void Timestamp_REQ_TYPE_011_Retains_Declared_Parameters()
    {
        var type = Assert.IsType<LogicalType.TimestampLogicalType>(
            LogicalType.Timestamp(TemporalUnit.Millisecond, timeZone: null));
        var value = ColumnValue.FromTimestamp(type, DateTimeOffset.UnixEpoch);
        Assert.Same(type, value.Type);
        Assert.Equal(TemporalUnit.Millisecond, Assert.IsType<LogicalType.TimestampLogicalType>(value.Type).Unit);
        Assert.Null(Assert.IsType<LogicalType.TimestampLogicalType>(value.Type).TimeZone);
    }
}
