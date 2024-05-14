namespace Tributech.SensorManager.Domain.ValueObjects;

// DataType: int, bool, double, datetime, hex
public record FieldType : IValueType<string>
{
    public string Value { get; }

    // null
    public static readonly FieldType None = new("none");
    public static readonly FieldType Int = new("int");
    public static readonly FieldType Bool = new("bool");
    public static readonly FieldType Double = new("double");
    public static readonly FieldType DateTime = new("datetime");
    public static readonly FieldType Hex = new("hex");
    public static readonly FieldType String = new("string");

    public FieldType(string value)
    {
        Value = value;
    }

    private FieldType()
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
        else if (this == String)
        {
            return true;
        }
        else
        {
            return null;
        }
    }

    private static readonly ValueTypeMap<FieldType> _mapping = [None, Int, Bool, Double, DateTime, Hex];

    public static FieldType Parse(string value)
    {
        // compare without case sensitivity
        if (_mapping.TryGetValue(value, out var dataType))
        {
            return dataType;
        }

        return _mapping.AddThreadSafe(value, new FieldType(value));
    }

    public static implicit operator (string Key, FieldType Value)(FieldType dataType)
    {
        return (dataType.Value, dataType);
    }

    // implicit from string
    public static implicit operator FieldType(string value)
    {
        return Parse(value);
    }

    public static implicit operator string(FieldType dataType)
    {
        return dataType.Value;
    }
}