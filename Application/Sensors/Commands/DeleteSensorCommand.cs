using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tributech.SensorManager.Application.Sensors.Commands;

public class DeleteSensorCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteSensorHandler : IRequestHandler<DeleteSensorCommand>
{
    private readonly ISensorContext _context;

    public DeleteSensorHandler(ISensorContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = await _context.Sensors.FindAsync(new object[] { request.Id }, cancellationToken);
        if (sensor == null) throw new InvalidOperationException("Sensor not found");

        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync(cancellationToken);
    }
}