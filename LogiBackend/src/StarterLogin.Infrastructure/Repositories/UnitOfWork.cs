using StarterLogin.Domain.Interfaces;
using StarterLogin.Infrastructure.Persistence;

namespace StarterLogin.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IUserRepository? _users;
    private IPokemonCardRepository? _cards;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IPokemonCardRepository Cards => _cards ??= new PokemonCardRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
