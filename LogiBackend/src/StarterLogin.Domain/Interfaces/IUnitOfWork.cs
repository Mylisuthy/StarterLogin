using System.Threading.Tasks;

namespace StarterLogin.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPokemonCardRepository Cards { get; }
    IMediaContentRepository Media { get; }
    IGenreRepository Genres { get; }
    IUserMediaRepository UserMedia { get; }
    Task<int> SaveChangesAsync();
}
