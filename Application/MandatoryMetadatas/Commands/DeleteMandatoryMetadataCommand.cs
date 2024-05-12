using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Commands;

// Delete
public class DeleteMandatoryMetadataCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteMandatoryMetadataCommandHandler : IRequestHandler<DeleteMandatoryMetadataCommand>
{
    private readonly ISensorContext _context;

    public DeleteMandatoryMetadataCommandHandler(ISensorContext context)
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