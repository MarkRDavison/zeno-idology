using Idology.UserInterface;

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
            .AddScoped<IPrototypeService<PersonPrototype, Person>, PersonPrototypeService>()
            .AddScoped<IPersonMovementService, PersonMovementService>()
            .AddScoped<IPersonSpawnService, PersonSpawnService>()
            .AddScoped<IPersonWorkService, PersonWorkService>()
            .AddScoped<IPrototypeService<ZombiePrototype, Zombie>, ZombiePrototypeService>()
            .AddScoped<IZombieSpawnService, ZombieSpawnService>()
            .AddScoped<IZombieWanderService, ZombieWanderService>()
            .AddScoped<IZombieMovementService, ZombieMovementService>()
            .AddScoped<IResourceService, ResourceService>();

        services.AddSingleton<TheInterface>(); // TODO: Temp

        return services;
    }
}
