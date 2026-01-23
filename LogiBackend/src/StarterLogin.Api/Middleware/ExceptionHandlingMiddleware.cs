using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StarterLogin.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        
        var (statusCode, title) = exception switch
        {
            StarterLogin.Application.Common.Exceptions.ValidationException => ((int)HttpStatusCode.BadRequest, "Error de Validación (Usuario)"),
            StarterLogin.Application.Common.Exceptions.AppException => ((int)HttpStatusCode.BadRequest, "Error de Aplicación"),
            ArgumentException => ((int)HttpStatusCode.BadRequest, "Solicitud Incorrecta"),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Acceso No Autorizado"),
            _ => ((int)HttpStatusCode.InternalServerError, "Error Interno del Sistema")
        };

        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        // En desarrollo podemos añadir más detalle si es un error de sistema
        if (statusCode == 500)
        {
            problemDetails.Extensions["debug_info"] = "Consulte los logs del servidor para más detalles.";
        }

        var response = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(response);
    }
}
