namespace StarterLogin.Application.Auth.Common;

public record AuthResponse(
    Guid Id,
    string UserName,
    string Email,
    string Token,
    string RefreshToken,
    IEnumerable<string> Roles
);
