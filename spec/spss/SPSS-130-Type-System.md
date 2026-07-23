# SPSS-130 — Type System

| Field | Value |
| --- | --- |
| Status | Draft |
| Category | Standards Track |
| Depends on | SPSS-001, SPSS-020, SPSS-030, SPSS-040 |
| Updates | None |
| Last updated | 2026-07-23 |

## Abstract

This document defines StreamPipe logical types and their values. It is format independent: an Arrow IPC, JSON, or future native format adapter must preserve these semantics, but each separately specifies its bytes.

## Type declaration

Every field declares one logical type before records are delivered. Types are immutable value descriptions. Values must conform to their field type and nullability.

`REQ-TYPE-001` — A type declaration **MUST** be immutable.

`REQ-TYPE-002` — A producer **MUST NOT** change a field type during an active stream.

`REQ-TYPE-003` — A consumer **MUST** reject a value that cannot be represented by its declared logical type.

## Scalar types

| Family | Logical types | Required semantics |
| --- | --- | --- |
| Boolean | `Boolean` | Exactly `true` or `false`. |
| Signed integer | `Int8`, `Int16`, `Int32`, `Int64` | Two's-complement signed integer with 8, 16, 32, or 64 value bits. |
| Unsigned integer | `UInt8`, `UInt16`, `UInt32`, `UInt64` | Non-negative integer with 8, 16, 32, or 64 value bits. |
| Floating point | `Float32`, `Float64` | IEEE 754 binary32 or binary64 values, including NaN and infinities. |
| Text | `Utf8` | A valid UTF-8 sequence. |
| Binary | `Binary` | An opaque finite byte sequence. |
| Identifier | `Uuid` | A 128-bit UUID value. |

`REQ-TYPE-004` — An integer value **MUST** be rejected if outside its declared type range.

`REQ-TYPE-005` — A text value **MUST** be valid UTF-8 at the logical-format boundary.

`REQ-TYPE-006` — A binary value **MUST NOT** be interpreted as text without an explicit format or application rule.

## Decimal

`Decimal(precision, scale)` represents a finite base-10 number whose unscaled integer has at most `precision` decimal digits and whose decimal point is `scale` digits from the right. Precision is 1 through 38; scale is 0 through precision.

`REQ-TYPE-007` — Decimal precision **MUST** be in the range 1–38.

`REQ-TYPE-008` — Decimal scale **MUST** be in the range 0 through precision.

`REQ-TYPE-009` — A decimal conversion **MUST NOT** silently round or truncate a value.

## Temporal types

`Date` is a proleptic Gregorian calendar date without a time or offset. `Time` is a time of day without a date or offset. `Timestamp(unit, timezone)` is an instant represented in one of `Second`, `Millisecond`, `Microsecond`, or `Nanosecond` units; `timezone` is either absent or an IANA time-zone identifier. `Duration(unit)` is a signed elapsed quantity using one of the same units.

`REQ-TYPE-010` — A timestamp timezone, when present, **MUST** be an IANA time-zone identifier.

`REQ-TYPE-011` — A format adapter **MUST** preserve timestamp unit and timezone without conversion unless an explicitly requested application conversion is applied.

`REQ-TYPE-012` — A temporal value **MUST NOT** acquire a local machine timezone implicitly.

## Nested types

`List(elementType, elementNullable)` is an ordered finite sequence. `Struct(fields)` is an ordered set of named fields. `Map(keyType, valueType, valueNullable)` is an ordered finite sequence of key/value entries; map keys are non-null.

`REQ-TYPE-013` — A list value **MUST** preserve element order.

`REQ-TYPE-014` — A struct value **MUST** contain exactly one value position for every declared nested field.

`REQ-TYPE-015` — A map key **MUST NOT** be null.

`REQ-TYPE-016` — Nested depth **MUST** not exceed the stream’s configured resource limit.

## Null

Null is a field-level absence value and is valid only when the field is nullable. Nested list elements and map values follow their declared nullability.

`REQ-TYPE-017` — Null **MUST** be represented distinctly from every non-null scalar or empty container value.

`REQ-TYPE-018` — A non-nullable field or nested position **MUST** reject null.

## .NET mapping

The .NET SDK maps scalar values to their same-width CLR primitive where available: `sbyte`, `short`, `int`, `long`, `byte`, `ushort`, `uint`, `ulong`, `float`, `double`, `string`, `ReadOnlyMemory<byte>`, and `Guid`. Decimal maps to `decimal` only when the value fits CLR decimal; otherwise a non-lossy representation is required. `DateOnly`, `TimeOnly`, `DateTimeOffset`, and `TimeSpan` are permitted adapters but must retain declared StreamPipe temporal parameters.

`REQ-TYPE-019` — A .NET mapping **MUST** reject or use a lossless representation when a StreamPipe value cannot be represented by the selected CLR type.

`REQ-TYPE-020` — A .NET public value contract **MUST** retain the declared logical type alongside a non-null value.

## Compatibility considerations

Adding a type requires capability negotiation defined by SPSS-100. Changing type meaning, decimal bounds, or temporal semantics is incompatible.

## Security considerations

Implementations must check nesting depth, lengths, precision, scale, and UTF-8 validity before allocating or traversing attacker-controlled data.

## Performance considerations

Validation should occur at bounded batch boundaries. Implementations may use format-native columnar representations but must preserve logical semantics.

## References

- [SPSS-020 — Data Model](SPSS-020-Data-Model.md)
- [SPSS-030 — Memory Model](SPSS-030-Memory-Model.md)
- [SPSS-040 — Public API](SPSS-040-Public-API.md)
