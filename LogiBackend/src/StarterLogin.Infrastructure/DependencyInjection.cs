using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarterLogin.Application.Common.Interfaces;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Infrastructure.Persistence;
using StarterLogin.Infrastructure.Repositories;
using StarterLogin.Infrastructure.Security;
using StarterLogin.Infrastructure.Services;

namespace StarterLogin.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IMediaContentRepository, MediaContentRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IUserMediaRepository, UserMediaRepository>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
