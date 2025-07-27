namespace Idology.Farm.Core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection services)
    {
        services
            .RegisterScene<FarmGameScene>();

        services
            .AddScoped<GameCamera>()
            .AddScoped<FarmGame>()
            .AddScoped<FarmGameRenderer>()
            .AddScoped<FarmGameData>();

        return services;
    }
}
