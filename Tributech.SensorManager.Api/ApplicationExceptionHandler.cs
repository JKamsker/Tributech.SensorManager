using Ardalis.GuardClauses;

using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Tributech.SensorManager.Api;

public class CustomExceptionHandler : IExceptionHandler
{
    public CustomExceptionHandler()
    {
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var handler = exception switch
        {
            ValidationException valEx => HandleValidationException(httpContext, valEx),
            NotFoundException notFoundEx => HandleNotFoundException(httpContext, notFoundEx),
            UnauthorizedAccessException unauthorizedEx => HandleUnauthorizedAccessException(httpContext, unauthorizedEx),
            System.ComponentModel.DataAnnotations.ValidationException systemValidationEx => HandleSystemValidationException(httpContext, systemValidationEx),

            _ => null
        };

        if (handler != null)
        {
            await handler;
            return true;
        }

        return false;
    }

    private async Task HandleSystemValidationException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Type = "tag:tributech.io,2024:validation-failed",
            Detail = exception.Message
        });
    }

    private async Task HandleValidationException(HttpContext httpContext, ValidationException exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var mappedErrors = exception.Errors
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(mappedErrors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Type = "tag:tributech.io,2024:validation-failed"
        });
    }

    private async Task HandleNotFoundException(HttpContext httpContext, NotFoundException ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "tag:tributech.io,2024:not-found",
            Title = "The specified resource was not found.",
            Detail = ex.Message
        });
    }

    private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "tag:tributech.io,2024:unauthorized"
        });
    }

    private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "tag:tributech.io,2024:forbidden"
        });
    }
}