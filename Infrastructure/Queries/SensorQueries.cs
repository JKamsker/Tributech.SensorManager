using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Application.Data;
using Tributech.SensorManager.Application.Queries;
using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Infrastructure.Queries;

internal class SensorQueries : ISensorQueries
{
    private readonly ISensorContext _context;

    public SensorQueries(ISensorContext context)
    {
        _context = context;
    }

    public async Task<IList<MandatoryMetadata>> GetMandatoryMetadataAsync(Sensor sensor)
    {
        return await _context.MandatoryMetadatas
            .Include(m => m.Metadata)
            .Where(m => m.SensorType == sensor.Type || m.SensorType == SensorType.Default)
            .ToListAsync();
    }
}