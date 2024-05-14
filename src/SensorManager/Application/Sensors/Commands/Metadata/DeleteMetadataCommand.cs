using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Tributech.SensorManager.Application.Sensors.Commands.Metadata;

// DeleteMetadataCommand
public record DeleteMetadataCommand(Guid SensorId, string Key) : IRequest;

public class DeleteMetadataHandler(ISensorContext context) : IRequestHandler<DeleteMetadataCommand>
{
    private readonly ISensorContext _context = context;

    public async Task Handle(DeleteMetadataCommand request, CancellationToken cancellationToken)
    {
        var sensor = await _context.Sensors
            .Include(s => s.Metadata)
            .FirstOrDefaultAsync(s => s.Id == request.SensorId, cancellationToken);

        if (sensor == null)
        {
            throw new Exception($"Sensor with id {request.SensorId} not found");
        }

        sensor.UnsetMetadata(request.Key);

        await _context.SaveChangesAsync(cancellationToken);
    }
}