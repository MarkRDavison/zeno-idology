namespace Idology.Outpost.Core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection services)
    {
        services
            .RegisterScene<GameScene>();

        services
            .AddScoped<Game>()
            .AddScoped<GameRenderer>()
            .AddScoped<GameData>();

        services
            .AddScoped<IPersonMovementService, PersonMovementService>()
            .AddScoped<IPersonSpawnService, PersonSpawnService>()
            .AddScoped<IPersonWorkService, PersonWorkService>()
            .AddScoped<IZombieSpawnService, ZombieSpawnService>()
            .AddScoped<IZombieWanderService, ZombieWanderService>()
            .AddScoped<IZombieMovementService, ZombieMovementService>()
            .AddScoped<IResourceService, ResourceService>();

        return services;
    }
}
