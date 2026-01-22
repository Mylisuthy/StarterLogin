using System.Threading.Tasks;

namespace StarterLogin.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPokemonCardRepository Cards { get; }
    Task<int> SaveChangesAsync();
}
