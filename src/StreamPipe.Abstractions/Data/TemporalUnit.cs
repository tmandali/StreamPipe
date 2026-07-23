namespace StreamPipe;

/// <summary>
/// Temporal unit for <see cref="LogicalType.Timestamp"/> and <see cref="LogicalType.Duration"/> (SPSS-130).
/// </summary>
public enum TemporalUnit
{
    /// <summary>Whole seconds.</summary>
    Second = 0,

    /// <summary>Milliseconds.</summary>
    Millisecond = 1,

    /// <summary>Microseconds.</summary>
    Microsecond = 2,

    /// <summary>Nanoseconds.</summary>
    Nanosecond = 3,
}
