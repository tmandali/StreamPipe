namespace StreamPipe;

/// <summary>
/// An immutable ordered collection of fields plus optional metadata (REQ-DATA-001, REQ-API-006, REQ-API-008).
/// </summary>
public sealed class DataSchema
{
    private DataSchema(IReadOnlyList<DataField> fields, SchemaMetadata metadata)
    {
        Fields = fields;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the ordered fields. Field order is significant (REQ-DATA-002).
    /// </summary>
    public IReadOnlyList<DataField> Fields { get; }

    /// <summary>
    /// Gets schema-level metadata. Metadata must not change core logical type meaning (REQ-DATA-004).
    /// </summary>
    public SchemaMetadata Metadata { get; }

    /// <summary>
    /// Creates a schema from the supplied fields.
    /// </summary>
    /// <param name="fields">Ordered fields; at least one is required.</param>
    /// <param name="metadata">Optional schema metadata.</param>
    /// <returns>An immutable schema.</returns>
    public static DataSchema Create(IEnumerable<DataField> fields, SchemaMetadata? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(fields);

        var list = fields as IList<DataField> ?? [.. fields];
        if (list.Count == 0)
        {
            // REQ-DATA-001
            throw new StreamPipeFormatException("A schema must define at least one field.");
        }

        var names = new HashSet<string>(StringComparer.Ordinal);
        var copy = new DataField[list.Count];
        for (var i = 0; i < list.Count; i++)
        {
            var field = list[i] ?? throw new ArgumentNullException(nameof(fields), "Schema fields must not be null.");
            // REQ-DATA-003 — byte-for-byte UTF-8 / ordinal comparison
            if (!names.Add(field.Name))
            {
                throw new StreamPipeFormatException($"Field names must be unique within one schema. Duplicate: '{field.Name}'.");
            }

            copy[i] = field;
        }

        return new DataSchema(copy, metadata ?? SchemaMetadata.Empty);
    }
}
