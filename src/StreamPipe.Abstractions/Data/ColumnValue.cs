using System.Diagnostics.CodeAnalysis;

namespace StreamPipe;

/// <summary>
/// One field value in a record, including an explicit null representation (REQ-API-007).
/// </summary>
public readonly struct ColumnValue : IEquatable<ColumnValue>
{
    private readonly object? _value;
    private readonly bool _hasValue;

    private ColumnValue(object? value, bool hasValue)
    {
        _value = value;
        _hasValue = hasValue;
    }

    /// <summary>
    /// Gets a null column value. Null is distinct from empty string, empty binary, and numeric zero (REQ-DATA-006).
    /// </summary>
    public static ColumnValue Null => default;

    /// <summary>
    /// Gets a value indicating whether this instance represents null.
    /// </summary>
    public bool IsNull => !_hasValue;

    /// <summary>Creates a boolean value.</summary>
    /// <param name="value">Boolean value.</param>
    /// <returns>A non-null column value.</returns>
    public static ColumnValue FromBoolean(bool value) => new(value, hasValue: true);

    /// <summary>Creates an 8-bit signed integer value.</summary>
    public static ColumnValue FromInt8(sbyte value) => new(value, hasValue: true);

    /// <summary>Creates a 16-bit signed integer value.</summary>
    public static ColumnValue FromInt16(short value) => new(value, hasValue: true);

    /// <summary>Creates a 32-bit signed integer value.</summary>
    public static ColumnValue FromInt32(int value) => new(value, hasValue: true);

    /// <summary>Creates a 64-bit signed integer value.</summary>
    public static ColumnValue FromInt64(long value) => new(value, hasValue: true);

    /// <summary>Creates an 8-bit unsigned integer value.</summary>
    public static ColumnValue FromUInt8(byte value) => new(value, hasValue: true);

    /// <summary>Creates a 16-bit unsigned integer value.</summary>
    public static ColumnValue FromUInt16(ushort value) => new(value, hasValue: true);

    /// <summary>Creates a 32-bit unsigned integer value.</summary>
    public static ColumnValue FromUInt32(uint value) => new(value, hasValue: true);

    /// <summary>Creates a 64-bit unsigned integer value.</summary>
    public static ColumnValue FromUInt64(ulong value) => new(value, hasValue: true);

    /// <summary>Creates a 32-bit floating-point value.</summary>
    public static ColumnValue FromFloat32(float value) => new(value, hasValue: true);

    /// <summary>Creates a 64-bit floating-point value.</summary>
    public static ColumnValue FromFloat64(double value) => new(value, hasValue: true);

    /// <summary>Creates a decimal value.</summary>
    public static ColumnValue FromDecimal(decimal value) => new(value, hasValue: true);

    /// <summary>Creates a UTF-8 text value. An empty string is not null.</summary>
    public static ColumnValue FromUtf8(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new ColumnValue(value, hasValue: true);
    }

    /// <summary>Creates a binary value. An empty sequence is not null.</summary>
    public static ColumnValue FromBinary(ReadOnlyMemory<byte> value) => new(value, hasValue: true);

    /// <summary>Creates a date value.</summary>
    public static ColumnValue FromDate(DateOnly value) => new(value, hasValue: true);

    /// <summary>Creates a time value.</summary>
    public static ColumnValue FromTime(TimeOnly value) => new(value, hasValue: true);

    /// <summary>Creates a timestamp value.</summary>
    public static ColumnValue FromTimestamp(DateTimeOffset value) => new(value, hasValue: true);

    /// <summary>Creates a duration value.</summary>
    public static ColumnValue FromDuration(TimeSpan value) => new(value, hasValue: true);

    /// <summary>Creates a UUID value.</summary>
    public static ColumnValue FromUuid(Guid value) => new(value, hasValue: true);

    /// <summary>Creates a list value as an ordered sequence of column values.</summary>
    public static ColumnValue FromList(ReadOnlyMemory<ColumnValue> values) => new(values, hasValue: true);

    /// <summary>Creates a struct value as an ordered sequence of column values.</summary>
    public static ColumnValue FromStruct(ReadOnlyMemory<ColumnValue> values) => new(values, hasValue: true);

    /// <summary>Creates a map value as ordered key/value pairs.</summary>
    public static ColumnValue FromMap(ReadOnlyMemory<KeyValuePair<ColumnValue, ColumnValue>> entries) =>
        new(entries, hasValue: true);

    /// <summary>Gets the boolean value.</summary>
    public bool GetBoolean() => Get<bool>();

    /// <summary>Gets the Int8 value.</summary>
    public sbyte GetInt8() => Get<sbyte>();

    /// <summary>Gets the Int16 value.</summary>
    public short GetInt16() => Get<short>();

    /// <summary>Gets the Int32 value.</summary>
    public int GetInt32() => Get<int>();

    /// <summary>Gets the Int64 value.</summary>
    public long GetInt64() => Get<long>();

    /// <summary>Gets the UInt8 value.</summary>
    public byte GetUInt8() => Get<byte>();

    /// <summary>Gets the UInt16 value.</summary>
    public ushort GetUInt16() => Get<ushort>();

    /// <summary>Gets the UInt32 value.</summary>
    public uint GetUInt32() => Get<uint>();

    /// <summary>Gets the UInt64 value.</summary>
    public ulong GetUInt64() => Get<ulong>();

    /// <summary>Gets the Float32 value.</summary>
    public float GetFloat32() => Get<float>();

    /// <summary>Gets the Float64 value.</summary>
    public double GetFloat64() => Get<double>();

    /// <summary>Gets the decimal value.</summary>
    public decimal GetDecimal() => Get<decimal>();

    /// <summary>Gets the UTF-8 text value.</summary>
    public string GetUtf8() => Get<string>();

    /// <summary>Gets the binary value.</summary>
    public ReadOnlyMemory<byte> GetBinary() => Get<ReadOnlyMemory<byte>>();

    /// <summary>Gets the date value.</summary>
    public DateOnly GetDate() => Get<DateOnly>();

    /// <summary>Gets the time value.</summary>
    public TimeOnly GetTime() => Get<TimeOnly>();

    /// <summary>Gets the timestamp value.</summary>
    public DateTimeOffset GetTimestamp() => Get<DateTimeOffset>();

    /// <summary>Gets the duration value.</summary>
    public TimeSpan GetDuration() => Get<TimeSpan>();

    /// <summary>Gets the UUID value.</summary>
    public Guid GetUuid() => Get<Guid>();

    /// <summary>Gets the list value.</summary>
    public ReadOnlyMemory<ColumnValue> GetList() => Get<ReadOnlyMemory<ColumnValue>>();

    /// <summary>Gets the struct value.</summary>
    public ReadOnlyMemory<ColumnValue> GetStruct() => Get<ReadOnlyMemory<ColumnValue>>();

    /// <summary>Gets the map value.</summary>
    public ReadOnlyMemory<KeyValuePair<ColumnValue, ColumnValue>> GetMap() =>
        Get<ReadOnlyMemory<KeyValuePair<ColumnValue, ColumnValue>>>();

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

        return Equals(_value, other._value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ColumnValue other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => _hasValue ? _value?.GetHashCode() ?? 0 : 0;

    /// <summary>Equality operator.</summary>
    public static bool operator ==(ColumnValue left, ColumnValue right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(ColumnValue left, ColumnValue right) => !left.Equals(right);

    private T Get<T>()
    {
        if (!_hasValue)
        {
            throw new StreamPipeFormatException("Cannot read a typed value from a null ColumnValue.");
        }

        if (_value is T typed)
        {
            return typed;
        }

        throw new StreamPipeFormatException($"ColumnValue does not contain a value of type '{typeof(T).Name}'.");
    }
}
