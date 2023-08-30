using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Model.Dto;
using ToDoList.Api.UseCase;

namespace ToDoList.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ActiviesController : ControllerBase
{
    private readonly IActivityUseCase _useCase;

    public ActiviesController(IActivityUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ActivityResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _useCase.GetById(id, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(ActivityResponseDto.FromDomain(result));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ActivityResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery]int page = 1, [FromQuery]int size = 20, CancellationToken cancellationToken = default)
    {
        var result = await _useCase.Get(default, page, size, cancellationToken);

        return Ok(result.Select(ActivityResponseDto.FromDomain));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] ActivyRequestDto model, CancellationToken cancellationToken = default)
    {
        var activy = (Model.Activity)model;

        var result = await _useCase.Add(activy,cancellationToken);

        if (!result)
            return BadRequest();

        return Created($"api/v1/activies/{activy.Id}", new { });
    }

    [HttpPost("{id}/next-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> NextStatus(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await _useCase.NextStatus(id, cancellationToken);

        if (!activity.HasValue)
            return NotFound();

        if (!activity.Value)
            return BadRequest(new { error = "Cannot update status with status is Finished or Cancelled." });

        return Ok();
    }

    [HttpPost("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await _useCase.Cancel(id, cancellationToken);

        if (!activity.HasValue)
            return NotFound();

        if (!activity.Value)
            return BadRequest(new { error = "Cannot update status with status is Finished or Cancelled." });

        return Ok();
    }
}
