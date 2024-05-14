using MediatR;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tributech.SensorManager.Application.Sensors.Common;

namespace Tributech.SensorManager.Application.Sensors.Commands;

public class DeleteSensorCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteSensorHandler(ISensorContext _context, IMemoryCache _memoryCache)
    : IRequestHandler<DeleteSensorCommand>
{
    public async Task Handle(DeleteSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = await _context.Sensors.FindAsync(new object[] { request.Id }, cancellationToken);
        if (sensor == null) throw new InvalidOperationException("Sensor not found");

        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove(CacheKeys.SensorsList);
        _memoryCache.Remove(CacheKeys.SensorItem(request.Id));
    }
}