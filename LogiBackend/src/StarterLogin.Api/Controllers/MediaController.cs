using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterLogin.Application.Multimedia.Common;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

namespace StarterLogin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MediaController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IUnitOfWork _unitOfWork; // Still needed for Age verification until refactored further
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IMemoryCache _cache;
    private const string MediaListCacheKey = "MediaList";

    public MediaController(ISender mediator, IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IMemoryCache cache)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
        _cache = cache;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MediaResponse>>> GetAll()
    {
        if (!_cache.TryGetValue(MediaListCacheKey, out IEnumerable<MediaResponse>? mediaResponses))
        {
            mediaResponses = await _mediator.Send(new GetMediaListQuery());

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            _cache.Set(MediaListCacheKey, mediaResponses, cacheOptions);
        }

        return Ok(mediaResponses);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<MediaResponse>> GetById(Guid id)
    {
        var response = await _mediator.Send(new GetMediaByIdQuery(id));
        if (response == null) return NotFound();

        // Security: Age verification for restricted content
        if (response.Rating == "R" || response.Rating == "18+")
        {
            if (User.Identity?.IsAuthenticated == false)
                return Forbid("Authentication required for restricted content.");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString != null)
            {
                var userId = Guid.Parse(userIdString);
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                
                if (user?.BirthDate != null)
                {
                    var age = DateTime.Today.Year - user.BirthDate.Value.Year;
                    if (user.BirthDate.Value.Date > DateTime.Today.AddYears(-age)) age--;
                    
                    if (age < 18)
                        return Forbid("This content is restricted for users under 18.");
                }
            }
        }

        return Ok(response);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MediaResponse>>> Search([FromQuery] string query)
    {
        var results = await _mediator.Send(new SearchMediaQuery(query));
        return Ok(results);
    }

    [HttpGet("{id}/recommendations")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MediaResponse>>> GetRecommendations(Guid id)
    {
        // For now, keeping simple recommendation logic in controller or could move to a query
        var content = await _unitOfWork.Media.GetByIdAsync(id);
        if (content == null) return NotFound();

        var media = await _unitOfWork.Media.GetAllAsync();
        var recommendations = media
            .Where(m => m.GenreId == content.GenreId && m.Id != id)
            .Take(5);

        return Ok(recommendations.Select(m => new MediaResponse(
            m.Id, m.Title, m.Description, m.ImageUrl, m.VideoUrl, m.Duration, m.ReleaseDate, m.Rating, m.GenreId, m.Genre?.Name ?? "Unknown", m.GetType().Name
        )));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MediaResponse>> Create(CreateMediaRequest request)
    {
        MediaContent content = request.ContentType.ToLower() switch
        {
            "movie" => Movie.Create(request.Title, request.Description, request.GenreId, request.ImageUrl, request.VideoUrl, request.Duration, request.ReleaseDate, request.Rating),
            "series" => Series.Create(request.Title, request.Description, request.GenreId, request.ImageUrl, request.ReleaseDate, request.Rating),
            "documentary" => Documentary.Create(request.Title, request.Description, request.GenreId, request.ImageUrl, request.VideoUrl, request.Duration, request.ReleaseDate, request.Rating),
            _ => throw new ArgumentException("Invalid content type")
        };

        await _unitOfWork.Media.AddAsync(content);
        await _unitOfWork.SaveChangesAsync();

        _cache.Remove(MediaListCacheKey);

        return CreatedAtAction(nameof(GetById), new { id = content.Id }, new MediaResponse(
            content.Id, content.Title, content.Description, content.ImageUrl, content.VideoUrl, content.Duration, content.ReleaseDate, content.Rating, content.GenreId, "Unknown", content.GetType().Name
        ));
    }

    [HttpPost("upload-image")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> UploadImage(IFormFile file)
    {
        var url = await _cloudinaryService.UploadImageAsync(file);
        if (url == null) return BadRequest("Upload failed");
        return Ok(new { url });
    }

    [HttpPost("upload-video")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> UploadVideo(IFormFile file)
    {
        var url = await _cloudinaryService.UploadVideoAsync(file);
        if (url == null) return BadRequest("Upload failed");
        return Ok(new { url });
    }
}
