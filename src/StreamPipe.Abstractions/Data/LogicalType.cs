namespace StreamPipe;

/// <summary>
/// Logical type family declared by a schema field before values are streamed (REQ-DATA-008).
/// Exact ranges and encodings are deferred to SPSS-130; this type preserves declared parameters (REQ-DATA-010).
/// </summary>
public abstract class LogicalType
{
    private protected LogicalType()
    {
    }

    /// <summary>Boolean logical type.</summary>
    public static LogicalType Boolean { get; } = new BooleanLogicalType();

    /// <summary>Signed 8-bit integer.</summary>
    public static LogicalType Int8 { get; } = new Int8LogicalType();

    /// <summary>Signed 16-bit integer.</summary>
    public static LogicalType Int16 { get; } = new Int16LogicalType();

    /// <summary>Signed 32-bit integer.</summary>
    public static LogicalType Int32 { get; } = new Int32LogicalType();

    /// <summary>Signed 64-bit integer.</summary>
    public static LogicalType Int64 { get; } = new Int64LogicalType();

    /// <summary>Unsigned 8-bit integer.</summary>
    public static LogicalType UInt8 { get; } = new UInt8LogicalType();

    /// <summary>Unsigned 16-bit integer.</summary>
    public static LogicalType UInt16 { get; } = new UInt16LogicalType();

    /// <summary>Unsigned 32-bit integer.</summary>
    public static LogicalType UInt32 { get; } = new UInt32LogicalType();

    /// <summary>Unsigned 64-bit integer.</summary>
    public static LogicalType UInt64 { get; } = new UInt64LogicalType();

    /// <summary>32-bit floating point.</summary>
    public static LogicalType Float32 { get; } = new Float32LogicalType();

    /// <summary>64-bit floating point.</summary>
    public static LogicalType Float64 { get; } = new Float64LogicalType();

    /// <summary>UTF-8 text.</summary>
    public static LogicalType Utf8 { get; } = new Utf8LogicalType();

    /// <summary>Opaque binary.</summary>
    public static LogicalType Binary { get; } = new BinaryLogicalType();

    /// <summary>Calendar date.</summary>
    public static LogicalType Date { get; } = new DateLogicalType();

    /// <summary>Time of day.</summary>
    public static LogicalType Time { get; } = new TimeLogicalType();

    /// <summary>Timestamp.</summary>
    public static LogicalType Timestamp { get; } = new TimestampLogicalType();

    /// <summary>Duration.</summary>
    public static LogicalType Duration { get; } = new DurationLogicalType();

    /// <summary>UUID identifier.</summary>
    public static LogicalType Uuid { get; } = new UuidLogicalType();

    /// <summary>
    /// Creates a decimal logical type, preserving precision and scale without lossy conversion (REQ-DATA-010).
    /// </summary>
    /// <param name="precision">Declared precision parameter.</param>
    /// <param name="scale">Declared scale parameter.</param>
    /// <returns>A decimal logical type.</returns>
    public static LogicalType Decimal(int precision, int scale) => new DecimalLogicalType(precision, scale);

    /// <summary>
    /// Creates a list logical type, preserving element type and nullability parameters (REQ-DATA-010).
    /// </summary>
    /// <param name="elementType">Element logical type.</param>
    /// <param name="elementNullable">Whether list elements may be null.</param>
    /// <returns>A list logical type.</returns>
    public static LogicalType List(LogicalType elementType, bool elementNullable) =>
        new ListLogicalType(elementType, elementNullable);

    /// <summary>
    /// Creates a struct logical type, preserving nested fields without lossy conversion (REQ-DATA-010).
    /// </summary>
    /// <param name="fields">Ordered nested fields.</param>
    /// <returns>A struct logical type.</returns>
    public static LogicalType Struct(IReadOnlyList<DataField> fields) => new StructLogicalType(fields);

    /// <summary>
    /// Creates a map logical type, preserving key/value parameters (REQ-DATA-010).
    /// </summary>
    /// <param name="keyType">Map key logical type.</param>
    /// <param name="valueType">Map value logical type.</param>
    /// <param name="valueNullable">Whether map values may be null.</param>
    /// <returns>A map logical type.</returns>
    public static LogicalType Map(LogicalType keyType, LogicalType valueType, bool valueNullable) =>
        new MapLogicalType(keyType, valueType, valueNullable);

    /// <summary>Boolean logical type instance.</summary>
    public sealed class BooleanLogicalType : LogicalType
    {
        internal BooleanLogicalType()
        {
        }
    }

    /// <summary>Signed 8-bit integer logical type.</summary>
    public sealed class Int8LogicalType : LogicalType
    {
        internal Int8LogicalType()
        {
        }
    }

    /// <summary>Signed 16-bit integer logical type.</summary>
    public sealed class Int16LogicalType : LogicalType
    {
        internal Int16LogicalType()
        {
        }
    }

    /// <summary>Signed 32-bit integer logical type.</summary>
    public sealed class Int32LogicalType : LogicalType
    {
        internal Int32LogicalType()
        {
        }
    }

    /// <summary>Signed 64-bit integer logical type.</summary>
    public sealed class Int64LogicalType : LogicalType
    {
        internal Int64LogicalType()
        {
        }
    }

    /// <summary>Unsigned 8-bit integer logical type.</summary>
    public sealed class UInt8LogicalType : LogicalType
    {
        internal UInt8LogicalType()
        {
        }
    }

    /// <summary>Unsigned 16-bit integer logical type.</summary>
    public sealed class UInt16LogicalType : LogicalType
    {
        internal UInt16LogicalType()
        {
        }
    }

    /// <summary>Unsigned 32-bit integer logical type.</summary>
    public sealed class UInt32LogicalType : LogicalType
    {
        internal UInt32LogicalType()
        {
        }
    }

    /// <summary>Unsigned 64-bit integer logical type.</summary>
    public sealed class UInt64LogicalType : LogicalType
    {
        internal UInt64LogicalType()
        {
        }
    }

    /// <summary>32-bit floating-point logical type.</summary>
    public sealed class Float32LogicalType : LogicalType
    {
        internal Float32LogicalType()
        {
        }
    }

    /// <summary>64-bit floating-point logical type.</summary>
    public sealed class Float64LogicalType : LogicalType
    {
        internal Float64LogicalType()
        {
        }
    }

    /// <summary>Decimal logical type with precision and scale.</summary>
    public sealed class DecimalLogicalType : LogicalType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalLogicalType"/> class.
        /// </summary>
        /// <param name="precision">Declared precision.</param>
        /// <param name="scale">Declared scale.</param>
        public DecimalLogicalType(int precision, int scale)
        {
            Precision = precision;
            Scale = scale;
        }

        /// <summary>Gets the declared precision.</summary>
        public int Precision { get; }

        /// <summary>Gets the declared scale.</summary>
        public int Scale { get; }
    }

    /// <summary>UTF-8 text logical type.</summary>
    public sealed class Utf8LogicalType : LogicalType
    {
        internal Utf8LogicalType()
        {
        }
    }

    /// <summary>Binary logical type.</summary>
    public sealed class BinaryLogicalType : LogicalType
    {
        internal BinaryLogicalType()
        {
        }
    }

    /// <summary>Date logical type.</summary>
    public sealed class DateLogicalType : LogicalType
    {
        internal DateLogicalType()
        {
        }
    }

    /// <summary>Time logical type.</summary>
    public sealed class TimeLogicalType : LogicalType
    {
        internal TimeLogicalType()
        {
        }
    }

    /// <summary>Timestamp logical type.</summary>
    public sealed class TimestampLogicalType : LogicalType
    {
        internal TimestampLogicalType()
        {
        }
    }

    /// <summary>Duration logical type.</summary>
    public sealed class DurationLogicalType : LogicalType
    {
        internal DurationLogicalType()
        {
        }
    }

    /// <summary>UUID logical type.</summary>
    public sealed class UuidLogicalType : LogicalType
    {
        internal UuidLogicalType()
        {
        }
    }

    /// <summary>List logical type.</summary>
    public sealed class ListLogicalType : LogicalType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListLogicalType"/> class.
        /// </summary>
        /// <param name="elementType">Element type.</param>
        /// <param name="elementNullable">Element nullability.</param>
        public ListLogicalType(LogicalType elementType, bool elementNullable)
        {
            ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            ElementNullable = elementNullable;
        }

        /// <summary>Gets the element logical type.</summary>
        public LogicalType ElementType { get; }

        /// <summary>Gets whether elements may be null.</summary>
        public bool ElementNullable { get; }
    }

    /// <summary>Struct logical type.</summary>
    public sealed class StructLogicalType : LogicalType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructLogicalType"/> class.
        /// </summary>
        /// <param name="fields">Ordered nested fields.</param>
        public StructLogicalType(IReadOnlyList<DataField> fields)
        {
            ArgumentNullException.ThrowIfNull(fields);
            if (fields.Count == 0)
            {
                throw new StreamPipeFormatException("A struct logical type must declare at least one field.");
            }

            Fields = fields;
        }

        /// <summary>Gets the ordered nested fields.</summary>
        public IReadOnlyList<DataField> Fields { get; }
    }

    /// <summary>Map logical type.</summary>
    public sealed class MapLogicalType : LogicalType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapLogicalType"/> class.
        /// </summary>
        /// <param name="keyType">Key type.</param>
        /// <param name="valueType">Value type.</param>
        /// <param name="valueNullable">Value nullability.</param>
        public MapLogicalType(LogicalType keyType, LogicalType valueType, bool valueNullable)
        {
            KeyType = keyType ?? throw new ArgumentNullException(nameof(keyType));
            ValueType = valueType ?? throw new ArgumentNullException(nameof(valueType));
            ValueNullable = valueNullable;
        }

        /// <summary>Gets the key logical type.</summary>
        public LogicalType KeyType { get; }

        /// <summary>Gets the value logical type.</summary>
        public LogicalType ValueType { get; }

        /// <summary>Gets whether values may be null.</summary>
        public bool ValueNullable { get; }
    }
}
