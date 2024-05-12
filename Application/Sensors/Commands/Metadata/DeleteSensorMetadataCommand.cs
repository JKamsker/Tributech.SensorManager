using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Tributech.SensorManager.Application.Sensors.Commands.Metadata;

// delete metadata
public record DeleteSensorMetadataCommand(Guid SensorId, string Key) : IRequest;

public class DeleteSensorMetadataHandler : IRequestHandler<DeleteSensorMetadataCommand>
{
    private readonly ISensorContext _context;

    public DeleteSensorMetadataHandler(ISensorContext context)
    {
        _context = context;
    }

    public Task Handle(DeleteSensorMetadataCommand request, CancellationToken cancellationToken)
    {
        var sensor = _context.Sensors
            .Include(s => s.Metadata)
            .FirstOrDefault(s => s.Id == request.SensorId);

        if (sensor == null)
        {
            throw new Exception($"Sensor with id {request.SensorId} not found");
        }

        sensor.UnsetMetadata(request.Key);

        return _context.SaveChangesAsync(cancellationToken);
    }
}