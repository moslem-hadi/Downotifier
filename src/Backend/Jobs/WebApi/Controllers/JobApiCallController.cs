
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.ApiCallJobCommandQuery;
using Application.ApiCallJobCommandQuery.Queries.GetPaged;
using Application.ApiCallJobCommandQuery.Queries.GetApiCallJob;
using Application.ApiCallJobCommandQuery.Commands.Create;
using Application.ApiCallJobCommandQuery.Commands.Update;
using Application.ApiCallJobCommandQuery.Commands.Delete;

namespace WebApi.Controllers;

//[Authorize]
//TODO: ooops, should be JobApiCalls
public class JobApiCallController : BaseApiController
{
    [HttpGet]
    public async Task<PaginatedList<ApiCallJobDto>> GetAll([FromQuery] GetApiCallJobsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ApiCallJobDto> GetById(int id)
    {
        return (await Mediator.Send(new GetApiCallJobsByIdQuery(id))) ?? throw new NotFoundException();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<int> Create(CreateApiCallJobCommand createCommand)
    {
        return await Mediator.Send(createCommand);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateApiCallJobCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        await Task.Delay(1000);
        await Mediator.Send(new DeleteApiCallJobCommand(id));

        return NoContent();
    }
}
