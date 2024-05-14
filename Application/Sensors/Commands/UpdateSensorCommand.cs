using MediatR;

using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Application.Sensors.Commands;

public class UpdateSensorCommand : IRequest<SensorVm>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class UpdateSensorHandler : IRequestHandler<UpdateSensorCommand, SensorVm>
{
    private readonly ISensorContext _context;

    public UpdateSensorHandler(ISensorContext context)
    {
        _context = context;
    }

    public async Task<SensorVm> Handle(UpdateSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = await _context.Sensors.FindAsync(new object[] { request.Id }, cancellationToken);
        if (sensor == null) throw new InvalidOperationException("Sensor not found");

        sensor.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return new SensorVm(sensor);
    }
}