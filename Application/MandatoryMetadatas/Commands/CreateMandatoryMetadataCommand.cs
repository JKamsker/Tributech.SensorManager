using MediatR;

using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Commands;

public class CreateMandatoryMetadataCommand : IRequest<MandatoryMetadataVm>
{
    public SensorType SensorType { get; set; }
    public ICollection<MandatoryMetadataItemVm> Metadata { get; set; } = [];
}

public class CreateMandatoryMetadataCommandHandler : IRequestHandler<CreateMandatoryMetadataCommand, MandatoryMetadataVm>
{
    private readonly ISensorContext _context;

    public CreateMandatoryMetadataCommandHandler(ISensorContext context)
    {
        _context = context;
    }

    public async Task<MandatoryMetadataVm> Handle(CreateMandatoryMetadataCommand request, CancellationToken cancellationToken)
    {
        // Assert: SensorType is unique
        if (await _context.MandatoryMetadatas.AnyAsync(m => m.SensorType == request.SensorType, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"MandatoryMetadata for SensorType '{request.SensorType}' already exists.");
        }

        var entity = new MandatoryMetadata(request.SensorType);
        entity.AddOrUpdateMetadata(request.Metadata.Select(m => m.AsEntity()));

        _context.MandatoryMetadatas.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new MandatoryMetadataVm(entity);
    }
}