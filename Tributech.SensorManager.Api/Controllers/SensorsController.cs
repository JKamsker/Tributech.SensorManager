﻿using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tributech.SensorManager.Application.Sensors.Commands;
using Tributech.SensorManager.Application.Sensors.Queries;
using Tributech.SensorManager.Application.Sensors.Queries.Common;
using Tributech.SensorManager.Domain.Entities;

namespace Tributech.SensorManager.Api.Controllers;

[ApiController]
[Route("sensors")]
public class SensorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SensorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // https://localhost:7067/Sensor
    // admin@tributech.io

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<SensorVm>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllSensorsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sensor>> Get(Guid id)
    {
        return Ok(await _mediator.Send(new GetSensorQuery { Id = id }));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Sensor>> Create(CreateSensorCommand command)
    {
        var sensor = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = sensor.Id }, sensor);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, SupportLevel3")]
    public async Task<IActionResult> Update(Guid id, UpdateSensorCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSensorCommand { Id = id });
        return NoContent();
    }
}