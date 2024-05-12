using Tributech.SensorManager.Domain.Entities;

namespace Tributech.SensorManager.Application.Queries;

public interface ISensorQueries
{
    Task<IList<MandatoryMetadata>> GetMandatoryMetadataAsync(Sensor sensor);
}