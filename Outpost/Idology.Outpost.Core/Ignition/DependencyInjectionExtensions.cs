namespace Idology.Outpost.Core.Ignition;

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

        services
            .AddScoped<IPrototypeService<WorkerPrototype, Worker>, WorkerPrototypeService>()
            .AddScoped<IPersonMovementService, PersonMovementService>()
            .AddScoped<IPersonSpawnService, PersonSpawnService>()
            .AddScoped<IPersonWorkService, PersonWorkService>()
            .AddScoped<IGuardDefenceService, GuardDefenceService>()
            .AddScoped<IPrototypeService<ZombiePrototype, Zombie>, ZombiePrototypeService>()
            .AddScoped<IZombieSpawnService, ZombieSpawnService>()
            .AddScoped<IZombieWanderService, ZombieWanderService>()
            .AddScoped<IZombieMovementService, ZombieMovementService>()
            .AddScoped<IZombieAttackService, ZombieAttackService>()
            .AddScoped<IResourceService, ResourceService>();

        return services;
    }
}
