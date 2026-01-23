using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Infrastructure.Persistence;

namespace StarterLogin.Infrastructure.Repositories;

public class UserMediaRepository : IUserMediaRepository
{
    private readonly ApplicationDbContext _context;

    public UserMediaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserMediaHistory?> GetHistoryAsync(Guid userId, Guid mediaId)
    {
        return await _context.UserMediaHistories
            .FirstOrDefaultAsync(h => h.UserId == userId && h.MediaContentId == mediaId);
    }

    public async Task<IEnumerable<UserMediaHistory>> GetUserHistoryAsync(Guid userId)
    {
        return await _context.UserMediaHistories
            .Include(h => h.MediaContent)
                .ThenInclude(m => m.Genre)
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.UpdatedAt ?? h.CreatedAt)
            .ToListAsync();
    }

    public async Task AddHistoryAsync(UserMediaHistory history)
    {
        await _context.UserMediaHistories.AddAsync(history);
    }

    public async Task<IEnumerable<UserFavorite>> GetUserFavoritesAsync(Guid userId)
    {
        return await _context.UserFavorites
            .Include(f => f.MediaContent)
                .ThenInclude(m => m.Genre)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<UserFavorite?> GetFavoriteAsync(Guid userId, Guid mediaId)
    {
        return await _context.UserFavorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.MediaContentId == mediaId);
    }

    public async Task AddFavoriteAsync(UserFavorite favorite)
    {
        await _context.UserFavorites.AddAsync(favorite);
    }

    public void RemoveFavorite(UserFavorite favorite)
    {
        _context.UserFavorites.Remove(favorite);
    }
}
