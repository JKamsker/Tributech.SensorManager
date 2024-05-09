using System.Text.Json;

using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Application;

public static class Extensions
{
    public static JsonSerializerOptions ConfigureJsonOptions(this JsonSerializerOptions options)
    {
        return options
            .ConfigureGetSensorQueryJsonOptions();
    }
}