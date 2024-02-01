using Malmasp.Options;

namespace Malmasp.Extensions;

public static class OptionsServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAppOptions(
        this IServiceCollection serviceCollection,
        ConfigurationManager configurationManager)
    {
        serviceCollection.Configure<PostgresqlOptions>(
            configurationManager.GetSection("Postgresql")
            );
        serviceCollection.Configure<JwtOptions>(
            configurationManager.GetSection("Jwt")
            );

        return serviceCollection;
    }
}