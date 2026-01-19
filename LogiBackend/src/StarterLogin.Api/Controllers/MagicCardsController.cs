using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarterLogin.Application.MagicCards.Commands.CreateMagicCard;
using StarterLogin.Application.MagicCards.Commands.DeleteMagicCard;
using StarterLogin.Application.MagicCards.Commands.PublishMagicCard;
using StarterLogin.Application.MagicCards.Commands.UpdateMagicCard;
using StarterLogin.Application.MagicCards.Queries;
using StarterLogin.Application.MagicCards.Queries.GetMagicCards;

namespace StarterLogin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MagicCardsController : ControllerBase
{
    private readonly ISender _mediator;

    public MagicCardsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous] // Or Authorize, but User View implies it might be public or just authenticated. I'll stick to authenticated.
    [Authorize] 
    public async Task<ActionResult<List<MagicCardDto>>> GetAll([FromQuery] bool includeDrafts = false)
    {
        // Enforce security: Only admins can see drafts
        if (includeDrafts && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var query = new GetMagicCardsQuery(includeDrafts);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> Create([FromForm] CreateMagicCardRequestDto request)
    {
        if (request.Image == null || request.Image.Length == 0)
        {
            return BadRequest("Image is required.");
        }

        using var stream = request.Image.OpenReadStream();
        var command = new CreateMagicCardCommand(
            request.Title,
            request.Description,
            stream,
            request.Image.FileName
        );

        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id }, id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateMagicCardRequestDto request)
    {
        Stream? stream = null;
        if (request.Image != null && request.Image.Length > 0)
        {
            stream = request.Image.OpenReadStream();
        }

        using (stream)
        {
            var command = new UpdateMagicCardCommand(
                id,
                request.Title,
                request.Description,
                stream,
                request.Image?.FileName
            );

            await _mediator.Send(command);
        }

        return NoContent();
    }

    [HttpPut("{id}/publish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Publish(Guid id, [FromBody] PublishMagicCardRequestDto request)
    {
        var command = new PublishMagicCardCommand(id, request.IsPublished);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteMagicCardCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}

public class CreateMagicCardRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile Image { get; set; } = default!;
}

public class UpdateMagicCardRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}

public class PublishMagicCardRequestDto
{
    public bool IsPublished { get; set; }
}
