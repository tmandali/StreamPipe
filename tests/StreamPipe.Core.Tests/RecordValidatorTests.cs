using StreamPipe;
using StreamPipe.Core;

namespace StreamPipe.Core.Tests;

public class RecordValidatorTests
{
    private static DataSchema CreateSchema() =>
        DataSchema.Create(
        [
            new DataField("id", LogicalType.Int32, isNullable: false),
            new DataField("name", LogicalType.Utf8, isNullable: true),
        ]);

    // REQ-DATA-011 — A record MUST contain exactly one value position for every schema field.
    [Fact]
    public void Validate_REQ_DATA_011_Rejects_Wrong_Length()
    {
        var schema = CreateSchema();
        var ex = Assert.Throws<StreamPipeFormatException>(() =>
            RecordValidator.Validate(schema, [ColumnValue.FromInt32(1)]));
        Assert.Contains("exactly one value position", ex.Message, StringComparison.Ordinal);
    }

    // REQ-DATA-012 — Value position n MUST conform to schema field position n.
    [Fact]
    public void Validate_REQ_DATA_012_Rejects_Type_Mismatch_At_Position()
    {
        var schema = CreateSchema();
        Assert.Throws<StreamPipeFormatException>(() =>
            RecordValidator.Validate(schema, [ColumnValue.FromUtf8("x"), ColumnValue.Null]));
    }

    // REQ-DATA-006 — A non-nullable field MUST NOT accept a null logical value.
    [Fact]
    public void Validate_REQ_DATA_006_Rejects_Null_For_NonNullable_Field()
    {
        var schema = CreateSchema();
        Assert.Throws<StreamPipeFormatException>(() =>
            RecordValidator.Validate(schema, [ColumnValue.Null, ColumnValue.FromUtf8("ok")]));
    }

    // REQ-API-014 — sink validation covers length, order, nullability, and logical type.
    [Fact]
    public void Validate_REQ_API_014_Accepts_Conforming_Record()
    {
        var schema = CreateSchema();
        RecordValidator.Validate(schema, [ColumnValue.FromInt32(7), ColumnValue.Null]);
    }
}
