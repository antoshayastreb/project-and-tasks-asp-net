using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Api.Middlewares;

/// <summary>
/// Глобальный обработчик ошибок.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Стандартное сообщение об ошибке
    /// </summary>
    private const string DefaultErrorMessage = "There was an unexpected error";

    /// <summary>
    /// Стандартная детализация ошибки
    /// </summary>
    private const string DefaultErrorDetails = "Please try again later";

    private readonly ILogger<GlobalExceptionHandler> _logger;

    /// <inheritdoc/>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        string errorMessage = DefaultErrorMessage;
        object errorDetail = DefaultErrorDetails;

        switch (exception)
        {
            case ValidationException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                errorMessage = "ValidationError";
                var errors = (exception as ValidationException)?.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                errorDetail = errors is null ? 
                    "Please validate your request" : errors;
                break;
            case NotFoundException:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                errorMessage = "NotFound";
                errorDetail = exception.Message ?? "Resource you are looking for not found";
                break;
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        await httpContext.Response.WriteAsJsonAsync(
            new { 
                error = errorMessage, 
                detail = errorDetail
            }, 
            cancellationToken
        );
        return true;
    }
}
