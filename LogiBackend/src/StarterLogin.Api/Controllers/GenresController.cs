using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterLogin.Application.Multimedia.Common;
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
public class GenresController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public GenresController(ISender mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<GenreResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetGenresQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<GenreResponse>> GetById(Guid id)
    {
        var genres = await _mediator.Send(new GetGenresQuery());
        var genre = genres.FirstOrDefault(g => g.Id == id);
        if (genre == null) return NotFound();
        return Ok(genre);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenreResponse>> Create(CreateGenreRequest request)
    {
        var genre = Genre.Create(request.Name, request.Description);
        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = genre.Id }, new GenreResponse(genre.Id, genre.Name, genre.Description));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateGenreRequest request)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(id);
        if (genre == null) return NotFound();

        genre.Update(request.Name, request.Description);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(id);
        if (genre == null) return NotFound();

        _unitOfWork.Genres.Remove(genre);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
