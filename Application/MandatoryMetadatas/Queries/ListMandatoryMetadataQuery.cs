using MediatR;

using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Infrastructure.Data;

namespace Tributech.SensorManager.Application.MandatoryMetadatas.Queries;

// Listall
public class ListMandatoryMetadataQuery : IRequest<IEnumerable<MandatoryMetadataVm>>
{
}

public class ListMandatoryMetadataQueryHandler : IRequestHandler<ListMandatoryMetadataQuery, IEnumerable<MandatoryMetadataVm>>
{
    private readonly SensorDbContext _context;

    public ListMandatoryMetadataQueryHandler(SensorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MandatoryMetadataVm>> Handle(ListMandatoryMetadataQuery request, CancellationToken cancellationToken)
    {
        return await _context.MandatoryMetadatas
            .Include(m => m.Metadata)
            .Select(m => new MandatoryMetadataVm(m))
            .ToListAsync(cancellationToken);
    }
}