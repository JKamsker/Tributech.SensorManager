using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Domain.Entities;

namespace Tributech.SensorManager.Application.Data;

public interface ISensorContext
{
    public DbSet<Sensor> Sensors { get; set; }

    public DbSet<MandatoryMetadata> MandatoryMetadatas { get; set; }

    Task SaveChangesAsync(CancellationToken token = default);
}