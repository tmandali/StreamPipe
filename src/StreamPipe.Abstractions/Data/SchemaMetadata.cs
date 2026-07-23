namespace StreamPipe;

/// <summary>
/// Immutable UTF-8 string key/value metadata associated with a schema or field (REQ-DATA-004).
/// Metadata is not a substitute for a protocol feature or type-system extension.
/// </summary>
public sealed class SchemaMetadata
{
    /// <summary>
    /// Gets an empty metadata instance.
    /// </summary>
    public static SchemaMetadata Empty { get; } = new(new Dictionary<string, string>(StringComparer.Ordinal));

    private readonly IReadOnlyDictionary<string, string> _entries;

    private SchemaMetadata(IReadOnlyDictionary<string, string> entries)
    {
        _entries = entries;
    }

    /// <summary>
    /// Gets the metadata entries. Keys are compared using ordinal (byte-for-byte) semantics.
    /// </summary>
    public IReadOnlyDictionary<string, string> Entries => _entries;

    /// <summary>
    /// Creates metadata from the supplied entries.
    /// </summary>
    /// <param name="entries">UTF-8 string keys and values.</param>
    /// <returns>An immutable metadata instance.</returns>
    public static SchemaMetadata Create(IEnumerable<KeyValuePair<string, string>> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var pair in entries)
        {
            ArgumentException.ThrowIfNullOrEmpty(pair.Key);
            ArgumentNullException.ThrowIfNull(pair.Value);

            if (!map.TryAdd(pair.Key, pair.Value))
            {
                throw new StreamPipeFormatException($"Duplicate metadata key '{pair.Key}'.");
            }
        }

        return map.Count == 0 ? Empty : new SchemaMetadata(map);
    }
}
