using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterLogin.Application.Multimedia.Common;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StarterLogin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserMediaController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UserMediaController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<UserHistoryResponse>>> GetHistory()
    {
        var userId = GetUserId();
        var history = await _unitOfWork.UserMedia.GetUserHistoryAsync(userId);
        return Ok(history.Select(h => new UserHistoryResponse(
            h.MediaContentId,
            h.MediaContent.Title,
            h.MediaContent.ImageUrl,
            h.WatchedTime,
            h.IsFinished,
            h.UpdatedAt ?? h.CreatedAt
        )));
    }

    [HttpPost("history")]
    public async Task<IActionResult> UpdateHistory(UpdateHistoryRequest request)
    {
        var userId = GetUserId();
        var history = await _unitOfWork.UserMedia.GetHistoryAsync(userId, request.MediaId);

        if (history == null)
        {
            history = new UserMediaHistory(userId, request.MediaId, request.WatchedTime, request.IsFinished);
            await _unitOfWork.UserMedia.AddHistoryAsync(history);
        }
        else
        {
            history.UpdateProgress(request.WatchedTime, request.IsFinished);
        }

        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("favorites")]
    public async Task<ActionResult<IEnumerable<FavoriteResponse>>> GetFavorites()
    {
        var userId = GetUserId();
        var favorites = await _unitOfWork.UserMedia.GetUserFavoritesAsync(userId);
        return Ok(favorites.Select(f => new FavoriteResponse(
            f.MediaContentId,
            f.MediaContent.Title,
            f.MediaContent.ImageUrl,
            f.MediaContent.Genre?.Name ?? "Unknown"
        )));
    }

    [HttpPost("favorites/{mediaId}")]
    public async Task<IActionResult> AddFavorite(Guid mediaId)
    {
        var userId = GetUserId();
        var favorite = await _unitOfWork.UserMedia.GetFavoriteAsync(userId, mediaId);

        if (favorite == null)
        {
            favorite = new UserFavorite(userId, mediaId);
            await _unitOfWork.UserMedia.AddFavoriteAsync(favorite);
            await _unitOfWork.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpDelete("favorites/{mediaId}")]
    public async Task<IActionResult> RemoveFavorite(Guid mediaId)
    {
        var userId = GetUserId();
        var favorite = await _unitOfWork.UserMedia.GetFavoriteAsync(userId, mediaId);

        if (favorite != null)
        {
            _unitOfWork.UserMedia.RemoveFavorite(favorite);
            await _unitOfWork.SaveChangesAsync();
        }

        return NoContent();
    }
}
