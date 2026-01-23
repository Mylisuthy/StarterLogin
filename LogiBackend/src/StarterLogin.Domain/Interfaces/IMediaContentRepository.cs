using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Domain.Interfaces;

public interface IMediaContentRepository
{
    Task<MediaContent?> GetByIdAsync(Guid id);
    Task<IEnumerable<MediaContent>> GetAllAsync();
    Task<IEnumerable<T>> GetByTypeAsync<T>() where T : MediaContent;
    Task AddAsync(MediaContent content);
    void Update(MediaContent content);
    void Remove(MediaContent content);
}
