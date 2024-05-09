using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System.Text.Json.Serialization;

using Tributech.SensorManager.Infrastructure.Data;

namespace Tributech.SensorManager.Application.Sensors.Commands.Metadata;

public record UpdateSensorMetadataCommand
(
    [property: JsonIgnore] Guid? SensorId,
    [property: JsonIgnore] string? Key,
    string Value
) : IRequest;

public class UpdateSensorMetadataHandler : IRequestHandler<UpdateSensorMetadataCommand>
{
    private readonly SensorDbContext _context;

    public UpdateSensorMetadataHandler(SensorDbContext context)
    {
        _context = context;
    }

    public Task Handle(UpdateSensorMetadataCommand request, CancellationToken cancellationToken)
    {
        var sensor = _context.Sensors
            .Include(s => s.Metadata)
            .FirstOrDefault(s => s.Id == request.SensorId);

        if (sensor == null)
        {
            throw new Exception($"Sensor with id {request.SensorId} not found");
        }

        sensor.SetMetadata(request.Key, request.Value);

        return _context.SaveChangesAsync(cancellationToken);
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