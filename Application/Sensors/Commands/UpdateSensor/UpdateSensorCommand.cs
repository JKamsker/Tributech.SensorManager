using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tributech.SensorManager.Infrastructure.Data;

namespace Tributech.SensorManager.Application.Sensors.Commands.UpdateSensor;
public class UpdateSensorCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class UpdateSensorHandler : IRequestHandler<UpdateSensorCommand>
{
    private readonly SensorDbContext _context;

    public UpdateSensorHandler(SensorDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = await _context.Sensors.FindAsync(new object[] { request.Id }, cancellationToken);
        if (sensor == null) throw new InvalidOperationException("Sensor not found");

        sensor.Name = request.Name;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
