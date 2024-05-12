using MediatR;

using Tributech.SensorManager.Application.Queries;
using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Domain.ValueTypes;

namespace Tributech.SensorManager.Application.Sensors.Commands;

public class CreateSensorCommand : IRequest<Sensor>
{
    public string Name { get; set; }
    public SensorType Type { get; set; }
    public IEnumerable<SensorMetadataVm>? Metadata { get; set; }
}

public class CreateSensorHandler(ISensorContext _context, ISensorQueries _sensorQueries) : IRequestHandler<CreateSensorCommand, Sensor>
{
    public async Task<Sensor> Handle(CreateSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = new Sensor
        {
            Name = request.Name,
            Type = request.Type,
        };

        if (request.Metadata != null)
        {
            sensor.SetMetadata(request.Metadata);
        }

        await CheckMandatoryMetadata(sensor);

        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync(cancellationToken);
        return sensor;
    }

    private async Task CheckMandatoryMetadata(Sensor sensor)
    {
        var mandatoryMetadata = await _sensorQueries.GetMandatoryMetadataAsync(sensor);
        sensor.CheckMandatoryMeatadata(mandatoryMetadata);
    }
}