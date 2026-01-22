using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterLogin.Application.Cards.Common;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterLogin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CardsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CardsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CardResponse>>> GetPublished()
    {
        var cards = await _unitOfWork.Cards.GetPublishedAsync();
        return Ok(cards.Select(MapToResponse));
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<CardResponse>>> GetAll()
    {
        var cards = await _unitOfWork.Cards.GetAllAsync();
        return Ok(cards.Select(MapToResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CardResponse>> GetById(Guid id)
    {
        var card = await _unitOfWork.Cards.GetByIdAsync(id);
        if (card == null) return NotFound();

        if (!card.IsPublished && !User.IsInRole("Admin"))
            return Forbid();

        return Ok(MapToResponse(card));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CardResponse>> Create(CreateCardRequest request)
    {
        var card = PokemonCard.Create(
            request.Title,
            request.ImageUrl,
            request.Description,
            request.HP,
            request.Attack,
            request.Defense
        );

        await _unitOfWork.Cards.AddAsync(card);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = card.Id }, MapToResponse(card));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateCardRequest request)
    {
        var card = await _unitOfWork.Cards.GetByIdAsync(id);
        if (card == null) return NotFound();

        card.Update(
            request.Title,
            request.ImageUrl,
            request.Description,
            request.HP,
            request.Attack,
            request.Defense
        );
        card.SetPublished(request.IsPublished);

        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var card = await _unitOfWork.Cards.GetByIdAsync(id);
        if (card == null) return NotFound();

        _unitOfWork.Cards.Remove(card);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}/publish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> TogglePublish(Guid id, [FromBody] bool isPublished)
    {
        var card = await _unitOfWork.Cards.GetByIdAsync(id);
        if (card == null) return NotFound();

        card.SetPublished(isPublished);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    private static CardResponse MapToResponse(PokemonCard card)
    {
        return new CardResponse(
            card.Id,
            card.Title,
            card.ImageUrl,
            card.Description,
            card.HP,
            card.Attack,
            card.Defense,
            card.IsPublished,
            card.CreatedAt
        );
    }
}
