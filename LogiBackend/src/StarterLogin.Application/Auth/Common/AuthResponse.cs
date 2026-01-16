namespace StarterLogin.Application.Auth.Common;

public record AuthResponse(
    Guid Id,
    string UserName,
    string Email,
    string Token,
    IEnumerable<string> Roles
);
