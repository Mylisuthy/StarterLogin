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
            throw new Common.Exceptions.ValidationException("El correo electrónico no se encuentra registrado.");

        // 2. Verificar password
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash.Value))
            throw new Common.Exceptions.ValidationException("La contraseña ingresada es incorrecta.");

        // 3. Generar Token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponse(
            user.Id,
            user.UserName,
            user.Email.Value,
            token,
            user.Roles.Select(r => r.Name)
        );
    }
}
