using System.Threading.Tasks;

namespace StarterLogin.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IMagicCardRepository MagicCards { get; }
    Task<int> SaveChangesAsync();
}
