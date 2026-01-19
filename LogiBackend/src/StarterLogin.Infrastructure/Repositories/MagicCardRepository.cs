using Microsoft.EntityFrameworkCore;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Infrastructure.Persistence;

namespace StarterLogin.Infrastructure.Repositories;

public class MagicCardRepository : IMagicCardRepository
{
    private readonly ApplicationDbContext _context;

    public MagicCardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MagicCard?> GetByIdAsync(Guid id)
    {
        return await _context.MagicCards.FindAsync(id);
    }

    public async Task<List<MagicCard>> GetAllAsync()
    {
        return await _context.MagicCards
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<MagicCard>> GetPublishedAsync()
    {
        return await _context.MagicCards
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(MagicCard magicCard)
    {
        await _context.MagicCards.AddAsync(magicCard);
    }

    public void Update(MagicCard magicCard)
    {
        _context.MagicCards.Update(magicCard);
    }

    public void Remove(MagicCard magicCard)
    {
        _context.MagicCards.Remove(magicCard);
    }
}
