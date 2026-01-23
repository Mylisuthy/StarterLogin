using StarterLogin.Domain.Entities;

namespace StarterLogin.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    RefreshToken GenerateRefreshToken(User user);
}
