using AutoMapper;
using Malmasp.Profiles;
using Malmasp.Services;

namespace Malmasp.Extensions;

public static class MappersServiceCollectionExtension
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var config = new MapperConfiguration((cfg) =>
        {
            cfg.AddProfile(new UserProfile(provider.GetRequiredService<HasherService>()));
        });
        services.AddSingleton(config.CreateMapper());

        return services;
    }
}