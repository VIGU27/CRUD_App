using CRUD_App.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_App.Middleware;

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
        catch (NotFoundException ex)
        {
            await WriteProblem(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ValidationException ex)
        {
            await WriteProblem(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblem(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblem(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = message
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}
