using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System.Text.Json.Serialization;

using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Commands;

// Update
public class UpdateMandatoryMetadataCommand : IRequest<MandatoryMetadataVm>
{
    [JsonIgnore]
    public Guid? Id { get; set; }

    public SensorType? Type { get; set; }

    public ICollection<MandatoryMetadataItemVm> Metadata { get; set; } = [];
}

public class UpdateMandatoryMetadataCommandHandler : IRequestHandler<UpdateMandatoryMetadataCommand, MandatoryMetadataVm>
{
    private readonly ISensorContext _context;

    public UpdateMandatoryMetadataCommandHandler(ISensorContext context)
    {
        _context = context;
    }

    public async Task<MandatoryMetadataVm> Handle(UpdateMandatoryMetadataCommand request, CancellationToken cancellationToken)
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

        return new MandatoryMetadataVm(entity);
    }
}

// FluentValidation: Id cannot be empty
public class UpdateMandatoryMetadataCommandValidator : AbstractValidator<UpdateMandatoryMetadataCommand>
{
    public UpdateMandatoryMetadataCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}