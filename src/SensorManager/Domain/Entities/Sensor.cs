using System.ComponentModel.DataAnnotations;

using Tributech.SensorManager.Domain.Abstractions;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Domain.Entities;

public class Sensor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public SensorType? Type { get; set; }

    public ICollection<SensorMetadata> Metadata { get; set; } = [];

    public void SetMetadata(IEnumerable<IKeyValuePair<string, string>> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        foreach (var item in metadata)
        {
            SetMetadata(item);
        }
    }

    public void SetMetadata(IKeyValuePair<string, string> metadata)
    {
        SetMetadata(metadata.Key, metadata.Value);
    }

    public void SetMetadata(string key, string value)
    {
        var metadata = Metadata.FirstOrDefault(m => m.Key == key);

        if (metadata == null)
        {
            Metadata.Add(new SensorMetadata { Key = key, Value = value });
        }
        else
        {
            metadata.Value = value;
        }
    }

    // unset
    public void UnsetMetadata(string key)
    {
        var metadata = Metadata.FirstOrDefault(m => m.Key == key);

        if (metadata != null)
        {
            Metadata.Remove(metadata);
        }
    }

    public void CheckMandatoryMeatadata(IEnumerable<MandatoryMetadata> items)
    {
        var metadataItems = items
            .Where(m => m.SensorType == Type || m.SensorType == SensorType.Default) // Restrict to the sensor type or default
            .OrderBy(x => x.SensorType == SensorType.Default ? 1 : 0) // Default has lower precedence
            .SelectMany(x => x.Metadata)
            .DistinctBy(x => x.Key);

        CheckMandatoryMetadata(metadataItems);
    }

    /// <summary>
    /// Checks if the mandatory metadata is set for the sensor.
    /// 1. Metadata is present = all good
    /// 2. Metadata is not present, but mandatory has default value = set default value
    /// 3. Metadata is not present, and mandatory has no default value = throw exception
    /// </summary>
    /// <param name="collection"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void CheckMandatoryMetadata(IEnumerable<MandatoryMetadataItem> collection)
    {
        foreach (var item in collection)
        {
            var metadata = Metadata.FirstOrDefault(m => m.Key == item.Key);
            CheckMandatoryMetadata(metadata, item);
        }
    }

    private void CheckMandatoryMetadata(SensorMetadata metadata, MandatoryMetadataItem item)
    {
        if (metadata != null)
        {
            if (item.Type.IsValid(metadata.Value) == false)
            {
                throw new ValidationException($"Mandatory metadata '{item.Key}' has invalid default value '{item.DefaultValue}' for sensor '{Name}'");
            }

            return;
        }

        if (item.DefaultValue == null)
        {
            throw new ValidationException($"Mandatory metadata '{item.Key}' is not set for sensor '{Name}'");
        }

        SetMetadata(item.Key, item.DefaultValue);
    }
}

public class SensorMetadata
{
    public string Key { get; set; }
    public string Value { get; set; }
}