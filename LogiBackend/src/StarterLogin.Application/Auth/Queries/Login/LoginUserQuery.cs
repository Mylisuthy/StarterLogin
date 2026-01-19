using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Application.Auth.Common;
using StarterLogin.Application.Common.Interfaces;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Domain.ValueObjects;

namespace StarterLogin.Application.Auth.Queries.Login;

public record LoginUserQuery(
    string Email,
    string Password
) : IRequest<AuthResponse>;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserQueryHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        // 1. Obtener usuario
        var user = await _unitOfWork.Users.GetByEmailAsync(Email.Create(request.Email));
        if (user == null)
            throw new Exception("Invalid credentials.");

        // 2. Verificar password
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash.Value))
            throw new Exception("Invalid credentials.");

        // 3. Generar Token y Refresh Token
        var token = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

        // 4. Guardar Refresh Token
        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse(
            user.Id,
            user.UserName,
            user.Email.Value,
            token,
            refreshToken.Token,
            user.Roles.Select(r => r.Name)
        );
    }
}
