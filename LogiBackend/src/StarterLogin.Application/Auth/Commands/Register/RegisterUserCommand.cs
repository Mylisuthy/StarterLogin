using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Application.Auth.Common;
using StarterLogin.Application.Common.Interfaces;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Domain.ValueObjects;

namespace StarterLogin.Application.Auth.Commands.Register;

public record RegisterUserCommand(
    string UserName,
    string Email,
    string Password
) : IRequest<AuthResponse>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar si ya existe
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(Email.Create(request.Email));
        if (existingUser != null)
            throw new StarterLogin.Application.Common.Exceptions.ValidationException("El correo electrónico ya se encuentra registrado por otro usuario.");

        // 2. Hashear password y crear dominio
        var passwordHash = PasswordHash.Create(_passwordHasher.Hash(request.Password));
        var user = User.Create(request.UserName, Email.Create(request.Email), passwordHash);

        // 3. Persistir
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // 4. Generar Token
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
