using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tributech.SensorManager.Domain.Entities;

using Tributech.SensorManager.Infrastructure.Data;
using System.Diagnostics;
using Tributech.SensorManager.Application.Sensors.Queries.Common;

namespace Tributech.SensorManager.Application.Sensors.Queries;

public class GetSensorQuery : IRequest<SensorVm>
{
    public Guid Id { get; set; }
}

public class GetSensorHandler : IRequestHandler<GetSensorQuery, SensorVm?>
{
    private readonly SensorDbContext _context;

    public GetSensorHandler(SensorDbContext context)
    {
        _context = context;
    }

    public async Task<SensorVm?> Handle(GetSensorQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Sensors
            .Include(s => s.Metadata)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        return result == null ? null : new SensorVm(result);
    }
}