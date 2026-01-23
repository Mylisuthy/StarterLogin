using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Infrastructure.Persistence;

namespace StarterLogin.Infrastructure.Repositories;

public class MediaContentRepository : IMediaContentRepository
{
    private readonly ApplicationDbContext _context;

    public MediaContentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MediaContent?> GetByIdAsync(Guid id)
    {
        return await _context.MediaContents
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<MediaContent>> GetAllAsync()
    {
        return await _context.MediaContents
            .Include(m => m.Genre)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetByTypeAsync<T>() where T : MediaContent
    {
        return await _context.Set<T>()
            .Include(m => m.Genre)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(MediaContent content)
    {
        await _context.MediaContents.AddAsync(content);
    }

    public void Update(MediaContent content)
    {
        _context.MediaContents.Update(content);
    }

    public void Remove(MediaContent content)
    {
        _context.MediaContents.Remove(content);
    }
}
