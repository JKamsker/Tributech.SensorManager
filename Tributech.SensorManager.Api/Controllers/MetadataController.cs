using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tributech.SensorManager.Application.Sensors.Commands.Metadata;
using Tributech.SensorManager.Application.Sensors.Queries;
using Tributech.SensorManager.Domain.Entities;

namespace Tributech.SensorManager.Api.Controllers;

[ApiController, ApiVersion("1")]
[Route("api/v{version:apiVersion}/sensors/{sensorId}/metadata")]
public class MetadataController : ControllerBase
{
    private readonly IMediator _mediator;

    public MetadataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<SensorMetadata>>> GetMetadata(Guid sensorId)
    {
        return Ok(await _mediator.Send(new GetMetadataQuery(sensorId)));
    }

    [HttpPut("{key}")]
    [Authorize(Roles = "Admin, SupportLevel3")]
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMetadata(Guid sensorId, string key)
    {
        await _mediator.Send(new DeleteMetadataCommand(sensorId, key));
        return NoContent();
    }
}