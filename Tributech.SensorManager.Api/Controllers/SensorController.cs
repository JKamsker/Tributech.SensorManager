using MediatR;

using Microsoft.AspNetCore.Mvc;

using Tributech.SensorManager.Application.Sensors.Commands.CreateSensor;
using Tributech.SensorManager.Application.Sensors.Commands.DeleteSensor;
using Tributech.SensorManager.Application.Sensors.Commands.UpdateSensor;
using Tributech.SensorManager.Application.Sensors.Queries.GetSensorQuery;
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
    public async Task<ActionResult<List<Sensor>>> GetAll()
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
[Route("[controller]")]
public class MetadataController : ControllerBase
{
    private readonly IMediator _mediator;

    public MetadataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/sensors/{sensorId}/metadata")]
    public async Task<ActionResult<List<SensorMetadata>>> GetMetadata(Guid sensorId)
    {
        return Ok(await _mediator.Send(new GetMetadataQuery { SensorId = sensorId }));
    }

    [HttpPut("/sensors/{sensorId}/metadata/{key}")]
    public async Task<IActionResult> UpdateMetadata(Guid sensorId, string key, UpdateMetadataCommand command)
    {
        if (sensorId != command.SensorId || key != command.Key)
        {
            return BadRequest("ID or Key mismatch");
        }
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("/sensors/{sensorId}/metadata/{key}")]
    public async Task<IActionResult> DeleteMetadata(Guid sensorId, string key)
    {
        await _mediator.Send(new DeleteMetadataCommand { SensorId = sensorId, Key = key });
        return NoContent();
    }
}
public class GetAllSensorsQuery : IRequest<List<Sensor>>
{
}
