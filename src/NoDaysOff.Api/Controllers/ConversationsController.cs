using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConversationDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetConversationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{conversationId}")]
    public async Task<ActionResult<ConversationDto>> GetById(int conversationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetConversationByIdQuery(conversationId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ConversationDto>> Create([FromBody] CreateConversationCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { conversationId = result.ConversationId }, result);
    }

    [HttpDelete("{conversationId}")]
    public async Task<ActionResult> Delete(int conversationId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteConversationCommand(conversationId, deletedBy), cancellationToken);
        return NoContent();
    }
}
