﻿using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Infrastructure.Data;

namespace Tributech.SensorManager.Application.Sensors.Commands;

public class CreateSensorCommand : IRequest<Sensor>
{
    public string Name { get; set; }
    public IEnumerable<SensorMetadataVm>? Metadata { get; set; }
}

public class CreateSensorHandler : IRequestHandler<CreateSensorCommand, Sensor>
{
    private readonly SensorDbContext _context;

    public CreateSensorHandler(SensorDbContext context)
    {
        _context = context;
    }

    public async Task<Sensor> Handle(CreateSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = new Sensor { Name = request.Name };
        foreach (var metadata in request.Metadata ?? [])
        {
            sensor.SetMetadata(metadata.Key, metadata.Value);
        }

        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync(cancellationToken);
        return sensor;
    }
}