using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tributech.SensorManager.Domain.Entities;

using System.Diagnostics;
using Tributech.SensorManager.Application.Sensors.Queries.Common;
using Microsoft.Extensions.Caching.Memory;
using Tributech.SensorManager.Application.Sensors.Common;

namespace Tributech.SensorManager.Application.Sensors.Queries;

public class GetSensorQuery : IRequest<SensorVm>
{
    public Guid Id { get; set; }
}

public class GetSensorHandler(ISensorContext _context, IMemoryCache _memoryCache)
    : IRequestHandler<GetSensorQuery, SensorVm?>
{
    public async Task<SensorVm?> Handle(GetSensorQuery request, CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync(CacheKeys.SensorItem(request.Id), async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var result = await _context.Sensors
                .Include(s => s.Metadata)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            return result == null ? null : new SensorVm(result);
        });
    }
}