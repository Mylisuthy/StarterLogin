using Microsoft.EntityFrameworkCore;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterLogin.Infrastructure.Repositories;

public class PokemonCardRepository : IPokemonCardRepository
{
    private readonly ApplicationDbContext _context;

    public PokemonCardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PokemonCard?> GetByIdAsync(Guid id)
    {
        return await _context.Cards.FindAsync(id);
    }

    public async Task<IEnumerable<PokemonCard>> GetAllAsync()
    {
        return await _context.Cards.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<PokemonCard>> GetPublishedAsync()
    {
        return await _context.Cards
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(PokemonCard card)
    {
        await _context.Cards.AddAsync(card);
    }

    public void Update(PokemonCard card)
    {
        _context.Cards.Update(card);
    }

    public void Remove(PokemonCard card)
    {
        _context.Cards.Remove(card);
    }
}
