namespace StreamPipe.Core;

/// <summary>
/// Validates records against a schema without materializing streams (REQ-DATA-011, REQ-DATA-012, REQ-API-014).
/// Exact value-range rules beyond CLR type presence are deferred to SPSS-130.
/// </summary>
public static class RecordValidator
{
    /// <summary>
    /// Validates that <paramref name="record"/> conforms to <paramref name="schema"/>.
    /// </summary>
    /// <param name="schema">Target schema.</param>
    /// <param name="record">Record values in schema field order.</param>
    /// <exception cref="StreamPipeFormatException">When length, nullability, or type kind does not match.</exception>
    public static void Validate(DataSchema schema, ReadOnlySpan<ColumnValue> record)
    {
        ArgumentNullException.ThrowIfNull(schema);

        if (record.Length != schema.Fields.Count)
        {
            // REQ-DATA-011
            throw new StreamPipeFormatException(
                $"A record must contain exactly one value position for every schema field. Expected {schema.Fields.Count}, received {record.Length}.");
        }

        for (var i = 0; i < record.Length; i++)
        {
            ValidateValue(schema.Fields[i], record[i], i);
        }
    }

    private static void ValidateValue(DataField field, ColumnValue value, int position)
    {
        if (value.IsNull)
        {
            if (!field.IsNullable)
            {
                // REQ-DATA-006
                throw new StreamPipeFormatException(
                    $"Non-nullable field '{field.Name}' at position {position} must not accept a null logical value.");
            }

            return;
        }

        if (!value.TryGetRawValue(out var raw))
        {
            throw new StreamPipeFormatException($"Field '{field.Name}' at position {position} has an invalid value representation.");
        }

        // REQ-DATA-012 — value position n must conform to schema field position n.
        if (!IsCompatible(field.Type, raw))
        {
            throw new StreamPipeFormatException(
                $"Value at position {position} does not conform to field '{field.Name}' of type '{field.Type.GetType().Name}'.");
        }
    }

    private static bool IsCompatible(LogicalType type, object raw) =>
        type switch
        {
            LogicalType.BooleanLogicalType => raw is bool,
            LogicalType.Int8LogicalType => raw is sbyte,
            LogicalType.Int16LogicalType => raw is short,
            LogicalType.Int32LogicalType => raw is int,
            LogicalType.Int64LogicalType => raw is long,
            LogicalType.UInt8LogicalType => raw is byte,
            LogicalType.UInt16LogicalType => raw is ushort,
            LogicalType.UInt32LogicalType => raw is uint,
            LogicalType.UInt64LogicalType => raw is ulong,
            LogicalType.Float32LogicalType => raw is float,
            LogicalType.Float64LogicalType => raw is double,
            LogicalType.DecimalLogicalType => raw is decimal,
            LogicalType.Utf8LogicalType => raw is string,
            LogicalType.BinaryLogicalType => raw is ReadOnlyMemory<byte>,
            LogicalType.DateLogicalType => raw is DateOnly,
            LogicalType.TimeLogicalType => raw is TimeOnly,
            LogicalType.TimestampLogicalType => raw is DateTimeOffset,
            LogicalType.DurationLogicalType => raw is TimeSpan,
            LogicalType.UuidLogicalType => raw is Guid,
            LogicalType.ListLogicalType => raw is ReadOnlyMemory<ColumnValue>,
            LogicalType.StructLogicalType => raw is ReadOnlyMemory<ColumnValue>,
            LogicalType.MapLogicalType => raw is ReadOnlyMemory<KeyValuePair<ColumnValue, ColumnValue>>,
            _ => false,
        };
}
