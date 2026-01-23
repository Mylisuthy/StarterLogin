using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Domain.Interfaces;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync(Guid id);
    Task<IEnumerable<Genre>> GetAllAsync();
    Task AddAsync(Genre genre);
    void Update(Genre genre);
    void Remove(Genre genre);
}
