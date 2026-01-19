using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Domain.Interfaces;

public interface IMagicCardRepository
{
    Task<MagicCard?> GetByIdAsync(Guid id);
    Task<List<MagicCard>> GetAllAsync();
    Task<List<MagicCard>> GetPublishedAsync();
    Task AddAsync(MagicCard magicCard);
    void Update(MagicCard magicCard);
    void Remove(MagicCard magicCard);
}
