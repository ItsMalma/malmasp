using System.Reflection;
using System.Text;
using System.Text.Json;
using Malmasp.Contexts;
using Malmasp.Extensions;
using Malmasp.Middlewares;
using Malmasp.Profiles;
using Malmasp.Services;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Malmasp;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var webApplicationBuilder = WebApplication.CreateBuilder(args);
        ConfigureHosts(webApplicationBuilder);
        ConfigureServices(webApplicationBuilder);

        var webApplication = webApplicationBuilder.Build();
        webApplication.UseSerilogRequestLogging();
        webApplication.UseExceptionHandler(_ => {});
        webApplication.UseStatusCodePages(AppStatusCodePages.Handler);
        webApplication.UseAuthentication();
        webApplication.UseAuthorization();
        webApplication.UseHttpsRedirection();
        webApplication.MapControllers();

        webApplication.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.ConfigureAppOptions(
            webApplicationBuilder.Configuration
            );
        webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>();
        webApplicationBuilder.Services.AddValidators();
        webApplicationBuilder.Services.AddSingleton<HasherService>();
        webApplicationBuilder.Services.AddMappers();
        webApplicationBuilder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });
        webApplicationBuilder.Services.AddExceptionHandler<AppExceptionHandler>();
        webApplicationBuilder.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = webApplicationBuilder.Configuration["Jwt:Issuer"],
                    ValidAudience = webApplicationBuilder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            webApplicationBuilder.Configuration["Jwt:Key"]!)),
                    ValidateIssuerSigningKey = true
                };
            });
        webApplicationBuilder.Services.AddAuthorization();
    }

    private static void ConfigureHosts(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Host.UseSerilog((_, config) =>
        {
            config.WriteTo.Console();
        });
    }
}