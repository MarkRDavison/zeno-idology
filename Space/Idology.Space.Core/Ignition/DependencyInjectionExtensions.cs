namespace Idology.Space.Core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection services)
    {
        services
            .RegisterScene<GameScene>();

        services
            .AddScoped<Infrastructure.Game>()
            .AddScoped<GameRenderer>()
            .AddScoped<GameData>();

        return services;
    }
}
