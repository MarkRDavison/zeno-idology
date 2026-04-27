namespace Idology.Conservation.Core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection services)
    {
        // TODO: handle seeding data for multiple cultures...
        services.AddLocalization(new()
        {
            { "WINDOW_TITLE", "Kakapo Conservation - 0.0.1 Alpha" },
            { "TITLE_SCREEN_TITLE", "Kakapo Conservation" },
            { "TITLE_SCREEN_START", "Start" },
            { "TITLE_SCREEN_LOAD", "Load" },
            { "TITLE_SCREEN_LOAD_DEV", "Load Dev" },
            { "TITLE_SCREEN_QUIT", "Quit" },
            { "TOP_BAR_DATE_FORMAT", "htt - MMM d yyyy" },
            { "TOP_BAR_KAKAPO_DETAILS", "Kakapo" },
            { "TOP_BAR_STAFF_DETAILS", "Staff" }
        });

        // Scenes
        services
            .RegisterScene<ConservationTitleScene>()
            .RegisterScene<ConservationGameScene>();

        // Subscenes

        services
            .AddTransient<KakapoDetailsSubScene>()
            .AddKeyedTransient<IUserInterfaceRoot, UserInterfaceRoot>(Constants.SubScene_KakapoDetails)
            .AddTransient<StaffDetialsSubScene>()
            .AddKeyedTransient<IUserInterfaceRoot, UserInterfaceRoot>(Constants.SubScene_StaffDetails);

        // Services
        services
            .AddScoped<ConservationGame>()
            .AddScoped<ConservationGameData>()
            .AddScoped<IConservationGameInteractionService, ConservationGameInteractionService>()
            .AddScoped<IGameDateTimeProvider, GameDateTimeProvider>()
            .AddScoped<IGameCommandService, GameCommandService>()
            .AddScoped<IEventRoutingService, EventRoutingService>();

        // Commands
        services
            .AddTransient<IGameCommandHandler<SetSubSceneGameCommand>, SetSubSceneGameCommandHandler>()
            .AddTransient<IGameCommandHandler<PopSubSceneGameCommand>, PopSubSceneGameCommandHandler>();

        // Widgets
        services
            .AddTransient<TopBarWidget>()
            .AddScoped<KakapoDetailsUiSubScenePanelWidget>()
            .AddScoped<StaffDetailsUiSubScenePanelWidget>();

        return services;
    }
}