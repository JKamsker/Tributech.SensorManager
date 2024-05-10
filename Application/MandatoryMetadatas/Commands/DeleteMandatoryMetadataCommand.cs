using MediatR;

using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Infrastructure.Data;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Commands;

// Delete
public class DeleteMandatoryMetadataCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteMandatoryMetadataCommandHandler : IRequestHandler<DeleteMandatoryMetadataCommand>
{
    private readonly SensorDbContext _context;

    public DeleteMandatoryMetadataCommandHandler(SensorDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMandatoryMetadataCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MandatoryMetadatas
            .Include(m => m.Metadata)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new InvalidOperationException($"MandatoryMetadata with Id '{request.Id}' not found.");
        }

        _context.MandatoryMetadatas.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}