using System.Threading.Tasks;

namespace StarterLogin.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
<<<<<<< HEAD
    IRefreshTokenRepository RefreshTokens { get; }
    IMagicCardRepository MagicCards { get; }
=======
    IPokemonCardRepository Cards { get; }
    IMediaContentRepository Media { get; }
    IGenreRepository Genres { get; }
    IUserMediaRepository UserMedia { get; }
>>>>>>> origin/test
    Task<int> SaveChangesAsync();
}
