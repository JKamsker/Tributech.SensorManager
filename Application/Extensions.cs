using System.Text.Json;

using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Application;

public static class Extensions
{
    public static JsonSerializerOptions ConfigureApplicationJsonOptions(this JsonSerializerOptions options)
    {
        // If this gets too much, split it up into namespaces.
        options.Converters.Add(new SingleMetadataConverter());
        return options;
    }
}