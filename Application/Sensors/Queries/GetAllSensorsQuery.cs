using MediatR;

using Microsoft.EntityFrameworkCore;

using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Application.Sensors.Queries;

public class GetAllSensorsQuery : IRequest<List<SensorVm>>
{
}

public class GetAllSensorsHandler : IRequestHandler<GetAllSensorsQuery, List<SensorVm>>
{
    private readonly ISensorContext _context;

    public GetAllSensorsHandler(ISensorContext context)
    {
        _context = context;
    }

    public async Task<List<SensorVm>> Handle(GetAllSensorsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Sensors
            .Include(s => s.Metadata)
            .Select(s => new SensorVm(s))
            .ToListAsync(cancellationToken);
    }
}