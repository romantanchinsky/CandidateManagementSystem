using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CandidateManagement.Api.Exceptions;

internal sealed class DomainExceptionHandler : IExceptionHandler
{
    IProblemDetailsService _problemDetailsService;
    public DomainExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DomainException)
        {
            return false;
        }
        httpContext.Response.StatusCode = exception switch
        {
            AccessDeniedDomainException => StatusCodes.Status403Forbidden,
            ConflictDomainException => StatusCodes.Status409Conflict,
            NotFoundDomainException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status400BadRequest
        };

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An error occured",
                Detail = exception.Message
            }
        });
    }
}