using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System.Text.Json.Serialization;

using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Domain.ValueTypes;

namespace Tributech.SensorManager.Application.Sensors.Commands.Metadata;

public record UpdateSensorMetadataCommand
(
    [property: JsonIgnore] Guid? SensorId,
    [property: JsonIgnore] string? Key,
    string Value
) : IRequest;

public class UpdateSensorMetadataHandler : IRequestHandler<UpdateSensorMetadataCommand>
{
    private readonly ISensorContext _context;

    public UpdateSensorMetadataHandler(ISensorContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSensorMetadataCommand request, CancellationToken cancellationToken)
    {
        _ = request.Key ?? throw new ArgumentNullException(nameof(request.Key));

        var sensor = _context.Sensors
            .Include(s => s.Metadata)
            .FirstOrDefault(s => s.Id == request.SensorId);

        if (sensor == null)
        {
            throw new Exception($"Sensor with id {request.SensorId} not found");
        }

        sensor.SetMetadata(request.Key, request.Value);

        await CheckMandatoryMetadata(sensor);

        await _context.SaveChangesAsync(cancellationToken);
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

// validator:
// we set the sensorid and key in the controller, but they cannot be null in the command
public class UpdateSensorMetadataCommandValidator : AbstractValidator<UpdateSensorMetadataCommand>
{
    public UpdateSensorMetadataCommandValidator()
    {
        RuleFor(x => x.SensorId).NotNull();
        RuleFor(x => x.Key).NotNull();
    }
}