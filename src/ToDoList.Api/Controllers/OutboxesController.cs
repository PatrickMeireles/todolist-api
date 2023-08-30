using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Model.Dto;
using ToDoList.Api.UseCase;

namespace ToDoList.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OutboxesController : ControllerBase
{
    private readonly IOutboxUseCase _usecase;

    public OutboxesController(IOutboxUseCase usecase)
    {
        _usecase = usecase;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OutboxResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken cancellationToken = default)
    {
        var result = await _usecase.Get(c => c != null, page, size, cancellationToken: cancellationToken);

        return Ok(result.Select(OutboxResponseDto.FromDomain));
    }
}
