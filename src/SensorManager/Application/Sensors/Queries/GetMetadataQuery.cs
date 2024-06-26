﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Application.Sensors.Common;

namespace Tributech.SensorManager.Application.Sensors.Queries;

public record GetMetadataQuery(Guid SensorId) : IRequest<List<SensorMetadataVm>>;

public class GetMetadataHandler : IRequestHandler<GetMetadataQuery, List<SensorMetadataVm>>
{
    private readonly ISensorContext _context;

    public GetMetadataHandler(ISensorContext context)
    {
        _context = context;
    }

    public async Task<List<SensorMetadataVm>> Handle(GetMetadataQuery request, CancellationToken cancellationToken)
    {
        var sensor = await _context.Sensors
            .Include(s => s.Metadata)
            .FirstOrDefaultAsync(s => s.Id == request.SensorId, cancellationToken);

        return sensor?.Metadata.Select(m => new SensorMetadataVm(m.Key, m.Value)).ToList() ?? new List<SensorMetadataVm>();
    }
}