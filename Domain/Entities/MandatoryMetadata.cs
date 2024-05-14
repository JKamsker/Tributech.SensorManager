using System.Collections;

using Tributech.SensorManager.Domain.Helpers;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Domain.Entities;

public class MandatoryMetadata
{
    public Guid Id { get; set; }

    public SensorType SensorType { get; set; }

    public ICollection<MandatoryMetadataItem> Metadata { get; set; } = [];

    public MandatoryMetadata(SensorType sensorType)
    {
        SensorType = sensorType;
    }

    public void AddOrUpdateMetadata(string key, ValueObjects.FieldType type, string? defaultValue)
    {
        var entry = Metadata.FirstOrDefault(m => m.Key == key);
        if (entry == null)
        {
            Metadata.Add(new MandatoryMetadataItem { Key = key, Type = type, DefaultValue = defaultValue });
        }
        else
        {
            entry.Type = type;
            entry.DefaultValue = defaultValue;
        }
    }

    public void AddOrUpdateMetadata(MandatoryMetadataItem metadata)
    {
        AddOrUpdateMetadata(metadata.Key, metadata.Type, metadata.DefaultValue);
    }

    public void AddOrUpdateMetadata(IEnumerable<MandatoryMetadataItem> metadata)
    {
        foreach (var item in metadata)
        {
            AddOrUpdateMetadata(item);
        }
    }

    public void SetMetadata(IEnumerable<MandatoryMetadataItem> metadata)
    {
        var keys = metadata.Select(m => m.Key).ToHashSet();
        var toDelete = Metadata.Where(m => !keys.Contains(m.Key)).ToList();

        foreach (var item in toDelete)
        {
            Metadata.Remove(item);
        }

        foreach (var item in metadata)
        {
            AddOrUpdateMetadata(item);
        }
    }
}

public class MandatoryMetadataItem
{
    public string Key { get; set; }
    public ValueObjects.FieldType Type { get; set; }
    public string? DefaultValue { get; set; }
}