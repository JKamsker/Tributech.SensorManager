using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Infrastructure.Data;

namespace Tributech.SensorManager.Application.Sensors.Queries.GetAllSensors;
public class GetAllSensorsQuery : IRequest<List<Sensor>>
{
}

public class GetAllSensorsHandler : IRequestHandler<GetAllSensorsQuery, List<Sensor>>
{
    private readonly SensorDbContext _context;

    public GetAllSensorsHandler(SensorDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sensor>> Handle(GetAllSensorsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Sensors
            .Include(s => s.Metadata)
            .ToListAsync(cancellationToken);
    }
}
