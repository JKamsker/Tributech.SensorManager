using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Domain.ValueTypes;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Common;

public class MandatoryMetadataVm
{
    public Guid Id { get; set; }

    public SensorType SensorType { get; set; }

    public ICollection<MandatoryMetadataItemVm> Metadata { get; set; } = [];

    public MandatoryMetadataVm(MandatoryMetadata entity)
    {
        Id = entity.Id;
        SensorType = entity.SensorType;
        Metadata = entity.Metadata.Select(m => new MandatoryMetadataItemVm(m)).ToList();
    }
}