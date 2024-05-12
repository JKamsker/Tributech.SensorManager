using MediatR;

using Microsoft.EntityFrameworkCore;

using System.Text.Json.Serialization;

using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Domain.ValueTypes;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Commands;

// Update
public class UpdateMandatoryMetadataCommand : IRequest
{
    [JsonIgnore]
    public Guid? Id { get; set; }

    public SensorType? Type { get; set; }

    public ICollection<MandatoryMetadataItemVm> Metadata { get; set; } = [];
}

public class UpdateMandatoryMetadataCommandHandler : IRequestHandler<UpdateMandatoryMetadataCommand>
{
    private readonly ISensorContext _context;

    public UpdateMandatoryMetadataCommandHandler(ISensorContext context)
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

        entity.SetMetadata(request.Metadata.Select(m => m.AsEntity()));

        await _context.SaveChangesAsync(cancellationToken);
    }
}