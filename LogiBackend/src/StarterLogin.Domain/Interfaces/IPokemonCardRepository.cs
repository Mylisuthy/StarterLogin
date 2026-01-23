using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Domain.Interfaces;

public interface IPokemonCardRepository
{
    Task<PokemonCard?> GetByIdAsync(Guid id);
    Task<IEnumerable<PokemonCard>> GetAllAsync();
    Task<IEnumerable<PokemonCard>> GetPublishedAsync();
    Task AddAsync(PokemonCard card);
    void Update(PokemonCard card);
    void Remove(PokemonCard card);
}
