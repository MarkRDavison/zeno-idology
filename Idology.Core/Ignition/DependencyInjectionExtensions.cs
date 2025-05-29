namespace Idology.Core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection services)
    {
        services
            .RegisterScene<TitleScene>()
            .RegisterScene<MedievalSheetExplorerScene>()
            .RegisterScene<GameScene>();
        //    .RegisterScene<OptionsScene>()
        //    .RegisterScene<ScoreScene>();

        services
            .AddTransient<PrototypeCreation>()
            .AddTransient<LevelRenderSystem>()
            .AddTransient<DebugUiSystem>()
            .AddTransient<CareerAllocatorSystem>()
            .AddTransient<WorkplaceProcessSystem>();


        services
            .AddSingleton<IPrototypeService<BuildingPrototype, BuildingComponent>, BuildingPrototypeService>()
            .AddSingleton<IPrototypeService<WorkerPrototype, WorkerComponent>>(_ => _.GetRequiredService<IWorkerPrototypeService>())
            .AddSingleton<IWorkerPrototypeService, WorkerPrototypeService>()
            .AddSingleton<IGameResourceService, GameResourceService>();

        return services;
    }
}
