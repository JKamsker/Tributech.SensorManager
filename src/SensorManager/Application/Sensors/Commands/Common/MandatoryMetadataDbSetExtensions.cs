using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Application.Sensors.Commands.Common;

internal static class MandatoryMetadataDbSetExtensions
{
    public static async Task<IList<MandatoryMetadata>> GetMandatoryMetadataAsync(this DbSet<MandatoryMetadata> context, Sensor sensor)
    {
        return await context
            .Include(m => m.Metadata)
            .Where(m => m.SensorType == sensor.Type || m.SensorType == SensorType.Default)
            .ToListAsync();
    }
}