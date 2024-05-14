using MediatR;

using Microsoft.Extensions.Caching.Memory;

using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Application.Sensors.Commands;

public class UpdateSensorCommand : IRequest<SensorVm>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class UpdateSensorHandler(ISensorContext _context, IMemoryCache _memoryCache)
    : IRequestHandler<UpdateSensorCommand, SensorVm>
{
    public async Task<SensorVm> Handle(UpdateSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = await _context.Sensors.FindAsync(new object[] { request.Id }, cancellationToken);
        if (sensor == null) throw new InvalidOperationException("Sensor not found");

        sensor.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove(CacheKeys.SensorsList);
        _memoryCache.Remove(CacheKeys.SensorItem(request.Id));

        return new SensorVm(sensor);
    }
}