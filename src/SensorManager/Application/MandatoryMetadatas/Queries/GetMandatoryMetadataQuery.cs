using MediatR;

using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Application.Data;
using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Queries;

// Queries:
public class GetMandatoryMetadataQuery : IRequest<MandatoryMetadataVm>
{
    public SensorType SensorType { get; set; }
}

public class GetMandatoryMetadataQueryHandler : IRequestHandler<GetMandatoryMetadataQuery, MandatoryMetadataVm>
{
    private readonly ISensorContext _context;

    public GetMandatoryMetadataQueryHandler(ISensorContext context)
    {
        _context = context;
    }

    public async Task<MandatoryMetadataVm?> Handle(GetMandatoryMetadataQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.MandatoryMetadatas
            .Include(m => m.Metadata)
            .FirstOrDefaultAsync(m => m.SensorType == request.SensorType, cancellationToken: cancellationToken);

        return result == null
            ? null
            : new MandatoryMetadataVm(result);
    }
}