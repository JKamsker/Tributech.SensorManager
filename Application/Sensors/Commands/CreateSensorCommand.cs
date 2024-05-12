using MediatR;

using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Application.Data;
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

public class CreateSensorHandler(ISensorContext _context) : IRequestHandler<CreateSensorCommand, Sensor>
{
    public async Task<Sensor> Handle(CreateSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = new Sensor
        {
            Name = request.Name,
            Type = request.Type,
        };
        foreach (var metadata in request.Metadata ?? [])
        {
            sensor.SetMetadata(metadata.Key, metadata.Value);
        }

        await CheckMandatoryMetadata(sensor);

        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync(cancellationToken);
        return sensor;
    }

    private async Task CheckMandatoryMetadata(Sensor sensor)
    {
        var mandatoryMetadata = await _context.MandatoryMetadatas
            .Include(m => m.Metadata)
            .Where(m => m.SensorType == sensor.Type || m.SensorType == SensorType.Default)
            .ToListAsync();

        sensor.CheckMandatoryMeatadata(mandatoryMetadata);
    }
}