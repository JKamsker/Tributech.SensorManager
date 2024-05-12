using System.Text.Json;

using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Application.Extensions;

public static class JsonSerializerExtensions
{
    public static JsonSerializerOptions ConfigureApplicationJsonOptions(this JsonSerializerOptions options)
    {
        // If this gets too much, split it up into namespaces.
        options.Converters.Add(new SingleMetadataConverter());
        return options;
    }
}

// dictionaryextensions
public static class DictionaryExtensions
{
    public static TValue? TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var value) ? value : default;
    }
}

// ienumerable foreach
public static class IEnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }
}