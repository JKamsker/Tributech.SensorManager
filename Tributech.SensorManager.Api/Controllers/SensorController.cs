using MediatR;

using Microsoft.AspNetCore.Mvc;

using Tributech.SensorManager.Application.Sensors.Commands;
using Tributech.SensorManager.Application.Sensors.Commands.Metadata;
using Tributech.SensorManager.Application.Sensors.Queries;
using Tributech.SensorManager.Application.Sensors.Queries.Common;
using Tributech.SensorManager.Domain.Entities;

namespace Tributech.SensorManager.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SensorController : ControllerBase
{
    private readonly IMediator _mediator;

    public SensorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
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
    public async Task<ActionResult<Sensor>> Create(CreateSensorCommand command)
    {
        var sensor = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = sensor.Id }, sensor);
    }

    [HttpPut("{id}")]
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
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSensorCommand { Id = id });
        return NoContent();
    }
}

[ApiController]
//[Route("[controller]")]
[Route("/sensors/{sensorId}/metadata")]
public class MetadataController : ControllerBase
{
    private readonly IMediator _mediator;

    public MetadataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet()]
    public async Task<ActionResult<List<SensorMetadata>>> GetMetadata(Guid sensorId)
    {
        return Ok(await _mediator.Send(new GetMetadataQuery(sensorId)));
    }

    [HttpPut("{key}")]
    public async Task<IActionResult> UpdateMetadata(Guid sensorId, string key, UpdateSensorMetadataCommand command)
    {
        command = command with
        {
            SensorId = sensorId,
            Key = key
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteMetadata(Guid sensorId, string key)
    {
        await _mediator.Send(new DeleteMetadataCommand(sensorId, key));
        return NoContent();
    }
}