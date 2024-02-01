using Malmasp.Dtos;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;

namespace Malmasp.Middlewares;

public static class AppStatusCodePages
{
    public static async Task Handler(StatusCodeContext context)
    {
        await context.HttpContext.Response.WriteAsJsonAsync(new Payload()
        {
            Error = ReasonPhrases.GetReasonPhrase(context.HttpContext.Response.StatusCode),
        });
    }
}
