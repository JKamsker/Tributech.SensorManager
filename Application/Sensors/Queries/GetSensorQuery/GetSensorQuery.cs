using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tributech.SensorManager.Domain.Entities;

using Tributech.SensorManager.Infrastructure.Data;

namespace Tributech.SensorManager.Application.Sensors.Queries.GetSensorQuery;
public class GetSensorQuery : IRequest<Sensor>
{
    public Guid Id { get; set; }
}

public class GetSensorHandler : IRequestHandler<GetSensorQuery, Sensor?>
{
    private readonly SensorDbContext _context;

    public GetSensorHandler(SensorDbContext context)
    {
        _context = context;
    }

    public async Task<Sensor?> Handle(GetSensorQuery request, CancellationToken cancellationToken)
    {
        return await _context.Sensors
            .Include(s => s.Metadata)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
    }
}


public class SensorVm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<SensorMetadataVm> Metadata { get; set; } = new();
}

public class SensorMetadataVm
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

/*
    JsonSerializer to serialize the List<SensorMetadataVm> or IEnumerable<SensorMetadataVm> to the following JSON:
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "name": "string",
        "metadata": {
            "id": "00000000-0000-0000-0000-000000000000",
            "key": "string",
            "value": "string"
        }
    }
 */


