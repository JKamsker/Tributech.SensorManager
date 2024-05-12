using Tributech.SensorManager.Domain.ValueTypes;

namespace Tributech.SensorManager.Domain.Entities;

public class Sensor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public SensorType? Type { get; set; }

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