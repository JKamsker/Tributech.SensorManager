using Tributech.SensorManager.Domain.Entities;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Common;

public class MandatoryMetadataItemVm
{
    public string Key { get; set; }
    public string Type { get; set; }
    public string? DefaultValue { get; set; }

    internal MandatoryMetadataItem AsEntity()
    {
        return new MandatoryMetadataItem
        {
            Key = Key,
            Type = Type,
            DefaultValue = DefaultValue
        };
    }

    public MandatoryMetadataItemVm()
    {
    }

    public MandatoryMetadataItemVm(MandatoryMetadataItem entity)
    {
        Key = entity.Key;
        Type = entity.Type;
        DefaultValue = entity.DefaultValue;
    }
}