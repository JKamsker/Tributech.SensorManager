using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Application.Sensors.Queries;

public class GetAllSensorsQuery : IRequest<List<SensorVm>>
{
}

public class GetAllSensorsHandler(ISensorContext _context, IMemoryCache _memoryCache)
    : IRequestHandler<GetAllSensorsQuery, List<SensorVm>>
{
    public async Task<List<SensorVm>> Handle(GetAllSensorsQuery request, CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync(CacheKeys.SensorsList, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            return await _context.Sensors
                .Include(s => s.Metadata)
                .Select(s => new SensorVm(s))
                .ToListAsync();
        }) ?? [];
    }
}