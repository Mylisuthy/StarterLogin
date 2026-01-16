namespace StarterLogin.Application.Auth.Common;

public record LoginRequest(string Email, string Password);

public record RegisterRequest(string UserName, string Email, string Password);
