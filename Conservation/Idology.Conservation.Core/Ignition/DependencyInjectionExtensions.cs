namespace Idology.Conservation.Core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection services)
    {
        services
            .RegisterScene<ConservationGameScene>();

        services
            .AddScoped<ConservationGame>()
            .AddScoped<ConservationGameRenderer>()
            .AddScoped<ConservationGameData>();

        return services;
    }
}