using StreamPipe;

namespace StreamPipe.Abstractions.Tests;

public class DataSchemaTests
{
    // REQ-DATA-001 — A schema MUST define at least one field.
    [Fact]
    public void Create_REQ_DATA_001_Throws_When_No_Fields()
    {
        var ex = Assert.Throws<StreamPipeFormatException>(() => DataSchema.Create(Array.Empty<DataField>()));
        Assert.Contains("at least one field", ex.Message, StringComparison.Ordinal);
    }

    // REQ-DATA-002 — A schema MUST preserve field order.
    [Fact]
    public void Create_REQ_DATA_002_Preserves_Field_Order()
    {
        var schema = DataSchema.Create(
        [
            new DataField("a", LogicalType.Int32, isNullable: false),
            new DataField("b", LogicalType.Utf8, isNullable: true),
        ]);

        Assert.Equal("a", schema.Fields[0].Name);
        Assert.Equal("b", schema.Fields[1].Name);
    }

    // REQ-DATA-003 — Field names MUST be unique under byte-for-byte UTF-8 comparison.
    [Fact]
    public void Create_REQ_DATA_003_Rejects_Duplicate_Field_Names()
    {
        Assert.Throws<StreamPipeFormatException>(() => DataSchema.Create(
        [
            new DataField("id", LogicalType.Int32, isNullable: false),
            new DataField("id", LogicalType.Utf8, isNullable: true),
        ]));
    }

    // REQ-DATA-003 — ordinal comparison is case-sensitive.
    [Fact]
    public void Create_REQ_DATA_003_Allows_Names_That_Differ_Only_By_Case()
    {
        var schema = DataSchema.Create(
        [
            new DataField("Id", LogicalType.Int32, isNullable: false),
            new DataField("id", LogicalType.Int32, isNullable: false),
        ]);

        Assert.Equal(2, schema.Fields.Count);
    }

    // REQ-DATA-005 — Each field MUST declare one logical type and one nullability value.
    [Fact]
    public void DataField_REQ_DATA_005_Captures_Type_And_Nullability()
    {
        var field = new DataField("name", LogicalType.Utf8, isNullable: true);
        Assert.Same(LogicalType.Utf8, field.Type);
        Assert.True(field.IsNullable);
    }

    // REQ-API-006 — public schema preserves field order, name, type, nullability, metadata.
    [Fact]
    public void Create_REQ_API_006_Preserves_Schema_Surface()
    {
        var metadata = SchemaMetadata.Create([new KeyValuePair<string, string>("owner", "tests")]);
        var fieldMeta = SchemaMetadata.Create([new KeyValuePair<string, string>("unit", "kg")]);
        var schema = DataSchema.Create(
            [new DataField("weight", LogicalType.Float64, isNullable: false, fieldMeta)],
            metadata);

        Assert.Equal("weight", schema.Fields[0].Name);
        Assert.Same(LogicalType.Float64, schema.Fields[0].Type);
        Assert.False(schema.Fields[0].IsNullable);
        Assert.Equal("kg", schema.Fields[0].Metadata.Entries["unit"]);
        Assert.Equal("tests", schema.Metadata.Entries["owner"]);
    }
}
