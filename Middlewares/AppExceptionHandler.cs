using System.Data.Common;
using Malmasp.Dtos;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Malmasp.Middlewares;

public class AppExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AppExceptionHandler> _logger;

    public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "{Message}", exception.Message);

        var statusCode = 500;
        var message = "Internal Server Error";

        if (exception is DbUpdateException { InnerException: not null } dbUpdateException)
        {
            exception = dbUpdateException.InnerException;
        }
        
        if (exception is DbException dbException)
        {
            (statusCode, message) = HandlePostgreSqlException(dbException);
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.WriteAsJsonAsync(new Payload()
        {
            Error = message,
        }, cancellationToken);
        
        return ValueTask.FromResult(true);
    }
    
    private (int statusCode, string message) HandlePostgreSqlException(DbException exception)
    {
        switch (exception.SqlState)
        {
            case PostgresErrorCodes.UniqueViolation:
                return (409, "Duplicated field");
            default:
                return (500, "Internal Server Error");
        }
    }
}