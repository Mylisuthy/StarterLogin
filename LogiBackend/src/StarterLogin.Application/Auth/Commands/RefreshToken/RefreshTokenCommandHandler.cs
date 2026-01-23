using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Application.Auth.Common;
using StarterLogin.Application.Common.Interfaces;
using StarterLogin.Domain.Interfaces;

namespace StarterLogin.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar el Refresh Token en BD
        var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.Token);

        if (existingToken == null)
            throw new Exception("Invalid Token");

        // 2. Validar expiración y revocación
        if (existingToken.IsRevoked || existingToken.ExpiryDate < DateTime.UtcNow)
        {
            // Opcional: Revocar cadena de tokens por seguridad si se usa uno inválido
            throw new Exception("Token expired or revoked");
        }

        // 3. Generar nuevos tokens
        var user = existingToken.User;
        var newAccessToken = _jwtTokenGenerator.GenerateToken(user);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

        // 4. Revocar el token actual (Rotación)
        existingToken.Revoke();
        
        // 5. Guardar el nuevo token
        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
        _unitOfWork.RefreshTokens.Update(existingToken);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse(
            user.Id,
            user.UserName,
            user.Email.Value,
            newAccessToken,
            newRefreshToken.Token,
            user.Roles.Select(r => r.Name)
        );
    }
}
