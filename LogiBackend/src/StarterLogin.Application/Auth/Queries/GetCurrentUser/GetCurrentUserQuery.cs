using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Application.Auth.Common;
using StarterLogin.Domain.Interfaces;

namespace StarterLogin.Application.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery(Guid UserId) : IRequest<AuthResponse>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        
        if (user == null)
            throw new UnauthorizedAccessException("User not found.");

        return new AuthResponse(
            user.Id,
            user.UserName,
            user.Email.Value,
            string.Empty, // No necesitamos enviar el token de nuevo aquí si ya está autenticado
            user.Roles.Select(r => r.Name)
        );
    }
}
