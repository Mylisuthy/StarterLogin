using StarterLogin.Domain.Interfaces;
using StarterLogin.Infrastructure.Persistence;

namespace StarterLogin.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IUserRepository? _users;
    private IPokemonCardRepository? _cards;
    private IMediaContentRepository? _media;
    private IGenreRepository? _genres;
    private IUserMediaRepository? _userMedia;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IPokemonCardRepository Cards => _cards ??= new PokemonCardRepository(_context);
    public IMediaContentRepository Media => _media ??= new MediaContentRepository(_context);
    public IGenreRepository Genres => _genres ??= new GenreRepository(_context);
    public IUserMediaRepository UserMedia => _userMedia ??= new UserMediaRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
