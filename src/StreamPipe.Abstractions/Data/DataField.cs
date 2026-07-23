namespace StreamPipe;

/// <summary>
/// An immutable schema field with name, logical type, nullability, and optional metadata (REQ-DATA-005).
/// </summary>
public sealed class DataField
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataField"/> class.
    /// </summary>
    /// <param name="name">Field name; uniqueness is enforced by <see cref="DataSchema"/>.</param>
    /// <param name="type">Logical type.</param>
    /// <param name="isNullable">Whether the field may contain a null logical value.</param>
    /// <param name="metadata">Optional field metadata.</param>
    public DataField(string name, LogicalType type, bool isNullable, SchemaMetadata? metadata = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Name = name;
        IsNullable = isNullable;
        Metadata = metadata ?? SchemaMetadata.Empty;
    }

    /// <summary>Gets the field name.</summary>
    public string Name { get; }

    /// <summary>Gets the logical type.</summary>
    public LogicalType Type { get; }

    /// <summary>Gets whether null values are permitted.</summary>
    public bool IsNullable { get; }

    /// <summary>Gets the field metadata.</summary>
    public SchemaMetadata Metadata { get; }
}
