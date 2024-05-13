using MediatR;

using Tributech.SensorManager.Application.Sensors.Commands.Common;
using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Application.Sensors.Queries.Common;
using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Application.Sensors.Commands;

public class CreateSensorCommand : IRequest<SensorVm>
{
    public string Name { get; set; }
    public SensorType Type { get; set; }
    public IEnumerable<SensorMetadataVm>? Metadata { get; set; }
}

public class CreateSensorHandler(ISensorContext _context) : IRequestHandler<CreateSensorCommand, SensorVm>
{
    public async Task<SensorVm> Handle(CreateSensorCommand request, CancellationToken cancellationToken)
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
        return new(sensor);
    }

    private async Task CheckMandatoryMetadata(Sensor sensor)
    {
        var mandatoryMetadata = await _context.MandatoryMetadatas.GetMandatoryMetadataAsync(sensor);
        sensor.CheckMandatoryMeatadata(mandatoryMetadata);
    }
}