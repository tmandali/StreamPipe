using System.Diagnostics.CodeAnalysis;

namespace StreamPipe;

/// <summary>
/// One field value in a record, including an explicit null representation (REQ-API-007, REQ-TYPE-017).
/// Non-null values retain their declared logical type (REQ-TYPE-020).
/// </summary>
public readonly struct ColumnValue : IEquatable<ColumnValue>
{
    private readonly object? _value;
    private readonly LogicalType? _type;
    private readonly bool _hasValue;

    private ColumnValue(LogicalType type, object value)
    {
        _type = type ?? throw new ArgumentNullException(nameof(type));
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _hasValue = true;
    }

    /// <summary>
    /// Gets a null column value. Null is distinct from empty string, empty binary, and numeric zero (REQ-DATA-006, REQ-TYPE-017).
    /// </summary>
    public static ColumnValue Null => default;

    /// <summary>
    /// Gets a value indicating whether this instance represents null.
    /// </summary>
    public bool IsNull => !_hasValue;

    /// <summary>
    /// Gets the declared logical type for a non-null value; <see langword="null"/> when <see cref="IsNull"/> is <see langword="true"/>.
    /// </summary>
    public LogicalType? Type => _type;

    /// <summary>Creates a boolean value.</summary>
    public static ColumnValue FromBoolean(bool value) => new(LogicalType.Boolean, value);

    /// <summary>Creates an 8-bit signed integer value.</summary>
    public static ColumnValue FromInt8(sbyte value) => new(LogicalType.Int8, value);

    /// <summary>Creates a 16-bit signed integer value.</summary>
    public static ColumnValue FromInt16(short value) => new(LogicalType.Int16, value);

    /// <summary>Creates a 32-bit signed integer value.</summary>
    public static ColumnValue FromInt32(int value) => new(LogicalType.Int32, value);

    /// <summary>Creates a 64-bit signed integer value.</summary>
    public static ColumnValue FromInt64(long value) => new(LogicalType.Int64, value);

    /// <summary>Creates an 8-bit unsigned integer value.</summary>
    public static ColumnValue FromUInt8(byte value) => new(LogicalType.UInt8, value);

    /// <summary>Creates a 16-bit unsigned integer value.</summary>
    public static ColumnValue FromUInt16(ushort value) => new(LogicalType.UInt16, value);

    /// <summary>Creates a 32-bit unsigned integer value.</summary>
    public static ColumnValue FromUInt32(uint value) => new(LogicalType.UInt32, value);

    /// <summary>Creates a 64-bit unsigned integer value.</summary>
    public static ColumnValue FromUInt64(ulong value) => new(LogicalType.UInt64, value);

    /// <summary>Creates a 32-bit floating-point value.</summary>
    public static ColumnValue FromFloat32(float value) => new(LogicalType.Float32, value);

    /// <summary>Creates a 64-bit floating-point value.</summary>
    public static ColumnValue FromFloat64(double value) => new(LogicalType.Float64, value);

    /// <summary>Creates a decimal value for the declared decimal logical type.</summary>
    /// <param name="type">Decimal logical type whose precision and scale are retained.</param>
    /// <param name="value">CLR decimal value; callers must ensure it fits without silent rounding (REQ-TYPE-009, REQ-TYPE-019).</param>
    public static ColumnValue FromDecimal(LogicalType.DecimalLogicalType type, decimal value) => new(type, value);

    /// <summary>Creates a UTF-8 text value. An empty string is not null.</summary>
    public static ColumnValue FromUtf8(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new ColumnValue(LogicalType.Utf8, value);
    }

    /// <summary>Creates a binary value. An empty sequence is not null.</summary>
    public static ColumnValue FromBinary(ReadOnlyMemory<byte> value) => new(LogicalType.Binary, value);

    /// <summary>Creates a date value.</summary>
    public static ColumnValue FromDate(DateOnly value) => new(LogicalType.Date, value);

    /// <summary>Creates a time value.</summary>
    public static ColumnValue FromTime(TimeOnly value) => new(LogicalType.Time, value);

    /// <summary>Creates a timestamp value for the declared timestamp logical type.</summary>
    /// <param name="type">Timestamp logical type whose unit and timezone are retained (REQ-TYPE-011).</param>
    /// <param name="value">Adapter instant value.</param>
    public static ColumnValue FromTimestamp(LogicalType.TimestampLogicalType type, DateTimeOffset value) =>
        new(type, value);

    /// <summary>Creates a duration value for the declared duration logical type.</summary>
    /// <param name="type">Duration logical type whose unit is retained.</param>
    /// <param name="value">Adapter duration value.</param>
    public static ColumnValue FromDuration(LogicalType.DurationLogicalType type, TimeSpan value) =>
        new(type, value);

    /// <summary>Creates a UUID value.</summary>
    public static ColumnValue FromUuid(Guid value) => new(LogicalType.Uuid, value);

    /// <summary>Creates a list value for the declared list logical type.</summary>
    public static ColumnValue FromList(LogicalType.ListLogicalType type, ReadOnlyMemory<ColumnValue> values) =>
        new(type, values);

    /// <summary>Creates a struct value for the declared struct logical type.</summary>
    public static ColumnValue FromStruct(LogicalType.StructLogicalType type, ReadOnlyMemory<ColumnValue> values) =>
        new(type, values);

    /// <summary>Creates a map value for the declared map logical type.</summary>
    public static ColumnValue FromMap(
        LogicalType.MapLogicalType type,
        ReadOnlyMemory<KeyValuePair<ColumnValue, ColumnValue>> entries) =>
        new(type, entries);

    /// <summary>Gets the boolean value.</summary>
    public bool GetBoolean() => Get<bool>(LogicalType.Boolean);

    /// <summary>Gets the Int8 value.</summary>
    public sbyte GetInt8() => Get<sbyte>(LogicalType.Int8);

    /// <summary>Gets the Int16 value.</summary>
    public short GetInt16() => Get<short>(LogicalType.Int16);

    /// <summary>Gets the Int32 value.</summary>
    public int GetInt32() => Get<int>(LogicalType.Int32);

    /// <summary>Gets the Int64 value.</summary>
    public long GetInt64() => Get<long>(LogicalType.Int64);

    /// <summary>Gets the UInt8 value.</summary>
    public byte GetUInt8() => Get<byte>(LogicalType.UInt8);

    /// <summary>Gets the UInt16 value.</summary>
    public ushort GetUInt16() => Get<ushort>(LogicalType.UInt16);

    /// <summary>Gets the UInt32 value.</summary>
    public uint GetUInt32() => Get<uint>(LogicalType.UInt32);

    /// <summary>Gets the UInt64 value.</summary>
    public ulong GetUInt64() => Get<ulong>(LogicalType.UInt64);

    /// <summary>Gets the Float32 value.</summary>
    public float GetFloat32() => Get<float>(LogicalType.Float32);

    /// <summary>Gets the Float64 value.</summary>
    public double GetFloat64() => Get<double>(LogicalType.Float64);

    /// <summary>Gets the decimal value.</summary>
    public decimal GetDecimal()
    {
        EnsureNonNull();
        if (_type is not LogicalType.DecimalLogicalType)
        {
            throw new StreamPipeFormatException("ColumnValue is not a decimal logical type.");
        }

        return (decimal)_value!;
    }

    /// <summary>Gets the UTF-8 text value.</summary>
    public string GetUtf8() => Get<string>(LogicalType.Utf8);

    /// <summary>Gets the binary value.</summary>
    public ReadOnlyMemory<byte> GetBinary() => Get<ReadOnlyMemory<byte>>(LogicalType.Binary);

    /// <summary>Gets the date value.</summary>
    public DateOnly GetDate() => Get<DateOnly>(LogicalType.Date);

    /// <summary>Gets the time value.</summary>
    public TimeOnly GetTime() => Get<TimeOnly>(LogicalType.Time);

    /// <summary>Gets the timestamp value.</summary>
    public DateTimeOffset GetTimestamp()
    {
        EnsureNonNull();
        if (_type is not LogicalType.TimestampLogicalType)
        {
            throw new StreamPipeFormatException("ColumnValue is not a timestamp logical type.");
        }

        return (DateTimeOffset)_value!;
    }

    /// <summary>Gets the duration value.</summary>
    public TimeSpan GetDuration()
    {
        EnsureNonNull();
        if (_type is not LogicalType.DurationLogicalType)
        {
            throw new StreamPipeFormatException("ColumnValue is not a duration logical type.");
        }

        return (TimeSpan)_value!;
    }

    /// <summary>Gets the UUID value.</summary>
    public Guid GetUuid() => Get<Guid>(LogicalType.Uuid);

    /// <summary>Gets the list value.</summary>
    public ReadOnlyMemory<ColumnValue> GetList()
    {
        EnsureNonNull();
        if (_type is not LogicalType.ListLogicalType)
        {
            throw new StreamPipeFormatException("ColumnValue is not a list logical type.");
        }

        return (ReadOnlyMemory<ColumnValue>)_value!;
    }

    /// <summary>Gets the struct value.</summary>
    public ReadOnlyMemory<ColumnValue> GetStruct()
    {
        EnsureNonNull();
        if (_type is not LogicalType.StructLogicalType)
        {
            throw new StreamPipeFormatException("ColumnValue is not a struct logical type.");
        }

        return (ReadOnlyMemory<ColumnValue>)_value!;
    }

    /// <summary>Gets the map value.</summary>
    public ReadOnlyMemory<KeyValuePair<ColumnValue, ColumnValue>> GetMap()
    {
        EnsureNonNull();
        if (_type is not LogicalType.MapLogicalType)
        {
            throw new StreamPipeFormatException("ColumnValue is not a map logical type.");
        }

        return (ReadOnlyMemory<KeyValuePair<ColumnValue, ColumnValue>>)_value!;
    }

    /// <summary>
    /// Attempts to get the boxed storage for validation helpers. Null values report <see langword="false"/>.
    /// </summary>
    /// <param name="value">Stored value when present.</param>
    /// <returns><see langword="true"/> when the value is non-null.</returns>
    public bool TryGetRawValue([NotNullWhen(true)] out object? value)
    {
        if (!_hasValue)
        {
            value = null;
            return false;
        }

        value = _value!;
        return true;
    }

    /// <inheritdoc />
    public bool Equals(ColumnValue other)
    {
        if (_hasValue != other._hasValue)
        {
            return false;
        }

        if (!_hasValue)
        {
            return true;
        }

        return Equals(_type, other._type) && Equals(_value, other._value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ColumnValue other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        _hasValue ? HashCode.Combine(_type, _value) : 0;

    /// <summary>Equality operator.</summary>
    public static bool operator ==(ColumnValue left, ColumnValue right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(ColumnValue left, ColumnValue right) => !left.Equals(right);

    private void EnsureNonNull()
    {
        if (!_hasValue)
        {
            throw new StreamPipeFormatException("Cannot read a typed value from a null ColumnValue.");
        }
    }

    private T Get<T>(LogicalType expectedType)
    {
        EnsureNonNull();
        if (!ReferenceEquals(_type, expectedType))
        {
            throw new StreamPipeFormatException(
                $"ColumnValue logical type '{_type?.GetType().Name}' does not match expected '{expectedType.GetType().Name}'.");
        }

        if (_value is T typed)
        {
            return typed;
        }

        throw new StreamPipeFormatException($"ColumnValue does not contain a value of type '{typeof(T).Name}'.");
    }
}
