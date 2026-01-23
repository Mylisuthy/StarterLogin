using StarterLogin.Domain.Interfaces;
using StarterLogin.Infrastructure.Persistence;

namespace StarterLogin.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IUserRepository? _users;
<<<<<<< HEAD
    private IRefreshTokenRepository? _refreshTokens;
    private IMagicCardRepository? _magicCards;
=======
    private IPokemonCardRepository? _cards;
    private IMediaContentRepository? _media;
    private IGenreRepository? _genres;
    private IUserMediaRepository? _userMedia;
>>>>>>> origin/test

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
<<<<<<< HEAD
    public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(_context);
    public IMagicCardRepository MagicCards => _magicCards ??= new MagicCardRepository(_context);
=======
    public IPokemonCardRepository Cards => _cards ??= new PokemonCardRepository(_context);
    public IMediaContentRepository Media => _media ??= new MediaContentRepository(_context);
    public IGenreRepository Genres => _genres ??= new GenreRepository(_context);
    public IUserMediaRepository UserMedia => _userMedia ??= new UserMediaRepository(_context);
>>>>>>> origin/test

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
