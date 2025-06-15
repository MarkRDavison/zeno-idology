namespace Idology.Space.Core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection services)
    {
        services
            .RegisterScene<GameScene>();

        services
            .AddScoped<GameCamera>()
            .AddScoped<Infrastructure.Game>()
            .AddScoped<GameRenderer>()
            .AddScoped<GameData>();

        services
            .AddScoped<SpaceCommandHandler>()
            .AddScoped<ISpaceCommandHandlerBase, SelectLocationCommandHandler>();

        return services;
    }
}
