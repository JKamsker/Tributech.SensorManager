using System.Text.Json.Serialization;
using System.Text.Json;

namespace Tributech.SensorManager.Application.Sensors.Queries.Common;

/*
    JsonSerializer to serialize the List<SensorMetadataVm> or IEnumerable<SensorMetadataVm> to the following JSON:
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "name": "string",
        "metadata": {
            "temperature": "75°F",
            "humidity": "50%"
        }
    }

    Deserialization also works for array notation:
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "name": "string",
        "metadata": [
            {
                "key": "temperature",
                "value": "75°F"
            },
            {
                "key": "humidity",
                "value": "50%"
            }
        ]
    }
 */

public class SingleMetadataConverter : JsonConverter<IEnumerable<SensorMetadataVm>>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(IEnumerable<SensorMetadataVm>)
            || typeToConvert.IsAssignableTo(typeof(IEnumerable<SensorMetadataVm>));
    }

    public override List<SensorMetadataVm> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return ReadAsObject(ref reader);
        }

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return ReadAsArray(ref reader);
        }

        throw new JsonException("Expected StartObject token.");
    }

    private List<SensorMetadataVm> ReadAsArray(ref Utf8JsonReader reader)
    {
        var list = new List<SensorMetadataVm>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var metadata = JsonSerializer.Deserialize<SensorMetadataVm>(ref reader);
                list.Add(metadata);
            }
        }

        return list;
    }

    private static List<SensorMetadataVm> ReadAsObject(ref Utf8JsonReader reader)
    {
        var list = new List<SensorMetadataVm>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var key = reader.GetString();
                reader.Read();
                var value = reader.GetString();
                list.Add(new SensorMetadataVm
                {
                    // Generate a new Guid for Id as we don't store Ids in the serialization format
                    Key = key,
                    Value = value
                });
            }
        }

        return list;
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<SensorMetadataVm> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var item in value)
        {
            writer.WriteString(item.Key, item.Value);
        }
        writer.WriteEndObject();
    }
}

// Extension class to add SingleMetadataConverter
public static class Extensions
{
    public static JsonSerializerOptions ConfigureGetSensorQueryJsonOptions(this JsonSerializerOptions options)
    {
        options.Converters.Add(new SingleMetadataConverter());
        return options;
    }
}