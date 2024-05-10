using MediatR;

using Microsoft.EntityFrameworkCore;

using System.Text.Json.Serialization;

using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Infrastructure.Data;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Commands;

// Update
public class UpdateMandatoryMetadataCommand : IRequest
{
    [JsonIgnore]
    public Guid? Id { get; set; }

    public ICollection<MandatoryMetadataItemVm> Metadata { get; set; } = [];
}

public class UpdateMandatoryMetadataCommandHandler : IRequestHandler<UpdateMandatoryMetadataCommand>
{
    private readonly SensorDbContext _context;

    public UpdateMandatoryMetadataCommandHandler(SensorDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMandatoryMetadataCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MandatoryMetadatas
            .Include(m => m.Metadata)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new InvalidOperationException($"MandatoryMetadata with Id '{request.Id}' not found.");
        }

        entity.AddOrUpdateMetadata(request.Metadata.Select(m => m.AsEntity()));

        await _context.SaveChangesAsync(cancellationToken);
    }
}