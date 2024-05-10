﻿using Tributech.SensorManager.Domain.ValueTypes;

namespace Tributech.SensorManager.Domain.Entities;

public class Sensor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public SensorType Type { get; set; }

    public ICollection<SensorMetadata> Metadata { get; set; } = [];

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
}

public class SensorMetadata
{
    public string Key { get; set; }
    public string Value { get; set; }
}

public class MandatoryMetadata
{
    public Guid Id { get; set; }

    public SensorType SensorType { get; set; }

    public ICollection<MandatoryMetadataItem> Metadata { get; set; } = [];

    public MandatoryMetadata(SensorType sensorType)
    {
        SensorType = sensorType;
    }

    public void AddOrUpdateMetadata(string key, string type, string? defaultValue)
    {
        //Metadata.Add(new MandatoryMetadataItem { Key = key, Type = type, DefaultValue = defaultValue });
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
}

public class MandatoryMetadataItem
{
    public string Key { get; set; }
    public string Type { get; set; }
    public string? DefaultValue { get; set; }
}