using MediatR;
using StarterLogin.Application.Auth.Common;

namespace StarterLogin.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<AuthResponse>;
