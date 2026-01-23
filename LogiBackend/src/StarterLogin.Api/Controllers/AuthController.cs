using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterLogin.Application.Auth.Commands.Register;
using StarterLogin.Application.Auth.Commands.RefreshToken;
using StarterLogin.Application.Auth.Common;
using StarterLogin.Application.Auth.Queries.GetCurrentUser;
using StarterLogin.Application.Auth.Queries.Login;

namespace StarterLogin.Api.Controllers;

/// <summary>
/// Controlador para la gestión de autenticación y usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="request">Datos del usuario a registrar.</param>
    /// <returns>Resultado de la autenticación con el token generado.</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.UserName, request.Email, request.Password);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Inicia sesión un usuario existente.
    /// </summary>
    /// <param name="request">Credenciales del usuario.</param>
    /// <returns>Token JWT e información del usuario.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var query = new LoginUserQuery(request.Email, request.Password);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene la información del usuario autenticado actualmente.
    /// </summary>
    /// <returns>Datos del usuario actual.</returns>
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AuthResponse>> GetMe()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var query = new GetCurrentUserQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand(request.Token);
        var authResult = await _mediator.Send(command);
        return Ok(authResult);
    }
}
