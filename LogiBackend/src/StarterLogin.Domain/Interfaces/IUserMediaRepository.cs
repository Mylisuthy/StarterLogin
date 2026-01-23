using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Domain.Interfaces;

public interface IUserMediaRepository
{
    Task<UserMediaHistory?> GetHistoryAsync(Guid userId, Guid mediaId);
    Task<IEnumerable<UserMediaHistory>> GetUserHistoryAsync(Guid userId);
    Task AddHistoryAsync(UserMediaHistory history);
    
    Task<IEnumerable<UserFavorite>> GetUserFavoritesAsync(Guid userId);
    Task<UserFavorite?> GetFavoriteAsync(Guid userId, Guid mediaId);
    Task AddFavoriteAsync(UserFavorite favorite);
    void RemoveFavorite(UserFavorite favorite);
}
