using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Domain.Extensions;

public static class JsonSerializerExtensions
{
    public static JsonSerializerOptions ConfigureDomainJsonOptions(this JsonSerializerOptions options)
    {
        // If this gets too much, split it up into namespaces.
        options.Converters.Add(new SensorTypeJsonConverter());
        return options;
    }
}