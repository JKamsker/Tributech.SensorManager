namespace Tributech.SensorManager.Api.Controllers;

using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

using Tributech.SensorManager.Application.MandatoryMetadatas.Commands;
using Tributech.SensorManager.Application.MandatoryMetadatas.Queries;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/sensors/metadata/mandatory")]
public class MandatoryMetadataController : ControllerBase
{
    private readonly IMediator _mediator;

    public MandatoryMetadataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // get by type
    [HttpGet("{sensorType}")]
    public async Task<IActionResult> GetByType(string sensorType)
    {
        if (string.IsNullOrWhiteSpace(sensorType))
        {
            return BadRequest("SensorType is required.");
        }

        var query = new GetMandatoryMetadataQuery { SensorType = sensorType };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // list all
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new ListMandatoryMetadataQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMandatoryMetadataCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{sensorType}")]
    public async Task<IActionResult> Update(string sensorType, [FromBody] UpdateMandatoryMetadataCommand command)
    {
        command.Type = sensorType;

        try
        {
            await _mediator.Send(command);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteMandatoryMetadataCommand { Id = id };
        try
        {
            await _mediator.Send(command);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}