namespace Tributech.SensorManager.Domain.ValueObjects;

// DataType: int, bool, double, datetime, hex
public record ValueType : IValueType<string>
{
    public string Value { get; }

    // null
    public static readonly ValueType None = new("none");
    public static readonly ValueType Int = new("int");
    public static readonly ValueType Bool = new("bool");
    public static readonly ValueType Double = new("double");
    public static readonly ValueType DateTime = new("datetime");
    public static readonly ValueType Hex = new("hex");

    public ValueType(string value)
    {
        Value = value;
    }

    private ValueType()
    {
    }

    public bool? IsValid(string value)
    {
        if (this == Int)
        {
            return int.TryParse(value, out _);
        }
        else if (this == Bool)
        {
            return bool.TryParse(value, out _);
        }
        else if (this == Double)
        {
            return double.TryParse(value, out _);
        }
        else if (this == DateTime)
        {
            return System.DateTime.TryParse(value, out _);
        }
        else if (this == Hex)
        {
            return value.Length % 2 == 0 && value.All(c => "0123456789ABCDEF".Contains(c, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            return null;
        }
    }

    private static readonly ValueTypeMap<ValueType> _mapping = [Int, Bool, Double, DateTime, Hex];

    public static ValueType Parse(string value)
    {
        // compare without case sensitivity
        if (_mapping.TryGetValue(value, out var dataType))
        {
            return dataType;
        }

        return _mapping.AddThreadSafe(value, new ValueType(value));
    }

    public static implicit operator (string Key, ValueType Value)(ValueType dataType)
    {
        return (dataType.Value, dataType);
    }

    // implicit from string
    public static implicit operator ValueType(string value)
    {
        return Parse(value);
    }

    public static implicit operator string(ValueType dataType)
    {
        return dataType.Value;
    }
}