using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Tributech.SensorManager.Domain.ValueObjects;
public record SensorType : IValueType<string>
{
    public string Value { get; }

    public static readonly SensorType Default = new("Default");
    public static readonly SensorType Temperature = new("Temperature");
    public static readonly SensorType Humidity = new("Humidity");
    public static readonly SensorType Altitute = new("Altitute");

    public SensorType(string value)
    {
        Value = value;
    }

    private SensorType()
    {
    }

    private static readonly ValueTypeMap<SensorType> _mapping = [Temperature, Humidity, Altitute];

    public static SensorType Parse(string value)
    {
        // compare without case sensitivity
        if (_mapping.TryGetValue(value, out var sensorType))
        {
            return sensorType;
        }

        return _mapping.AddThreadSafe(value, new SensorType(value));
    }

    public static implicit operator (string Key, SensorType Value)(SensorType sensorType)
    {
        return (sensorType.Value, sensorType);
    }

    // implicit from string
    public static implicit operator SensorType(string value)
    {
        return Parse(value);
    }
}

public interface IValueType<T>
{
    T Value { get; }
}

internal class ValueTypeMap<TStatus> : IEnumerable<(string Key, TStatus Value)>
{
    private Dictionary<string, TStatus> _mappings = new(StringComparer.OrdinalIgnoreCase);

    internal int Count => _mappings.Count;

    public ValueTypeMap<TStatus> Add(IValueType<string> value)
    {
        _mappings.Add(value.Value, (TStatus)value);
        return this;
    }

    public ValueTypeMap<TStatus> Add(string key, TStatus value)
    {
        _mappings.Add(key, value);
        return this;
    }

    public ValueTypeMap<TStatus> TryAdd(string key, TStatus value)
    {
        _mappings.TryAdd(key, value);
        return this;
    }

    public TStatus Get(string key)
    {
        return _mappings[key];
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out TStatus value)
    {
        return _mappings.TryGetValue(key, out value);
    }

    /// <summary>
    /// Adds a value to the dictionary in a thread-safe way without locks.
    /// </summary>
    /// <param name="key">The key to add</param>
    /// <param name="value">The value to add</param>
    /// <returns>The added value or the existing value if another thread added it already</returns>
    /// <remarks>
    /// Yes, I know that this is not the most efficient way to add values to a dictionary.
    /// But it is thread-safe without locks and adding should be an operation that is not called very often.
    /// </remarks>
    public TStatus AddThreadSafe(string key, TStatus value)
    {
        while (true)
        {
            var currentMap = Volatile.Read(ref _mappings);

            var newMap = new Dictionary<string, TStatus>(currentMap, StringComparer.OrdinalIgnoreCase);

            // true, if another thread added the value already
            var alreadyAdded = !newMap.TryAdd(key, value);
            if (alreadyAdded)
            {
                return newMap[key];
            }

            if (Interlocked.CompareExchange(ref _mappings, newMap, currentMap) == currentMap)
            {
                return value;
            }
        }
    }

    public IEnumerable<(string Key, TStatus Value)> Enumerate()
    {
        return _mappings.Select(kvp => (kvp.Key, kvp.Value));
    }

    IEnumerator<(string Key, TStatus Value)> IEnumerable<(string Key, TStatus Value)>.GetEnumerator()
        => Enumerate().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Enumerate().GetEnumerator();
}

public class SensorTypeJsonConverter : JsonConverter<SensorType>
{
    public override SensorType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return SensorType.Parse(value);
    }

    public override void Write(Utf8JsonWriter writer, SensorType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}