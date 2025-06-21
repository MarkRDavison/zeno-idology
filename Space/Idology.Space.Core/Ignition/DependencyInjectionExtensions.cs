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
            .AddScoped<GameData>()
            .AddScoped<IInputService, InputService>()
            .AddScoped<IPathfindingService, PathfindingService>();

        services
            .AddScoped<SpaceCommandHandler>()
            .AddScoped<ISpaceCommandHandlerBase, SelectLocationCommandHandler>()
            .AddScoped<ISpaceCommandHandlerBase, FindPathCommandHandler>();

        return services;
    }
}
