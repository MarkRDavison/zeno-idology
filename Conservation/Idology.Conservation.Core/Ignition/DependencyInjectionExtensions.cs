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
            { "TITLE_SCREEN_SIMULATE", "Simulate" },
            { "TITLE_SCREEN_LOAD", "Load" },
            { "TITLE_SCREEN_LOAD_DEV", "Load Dev" },
            { "TITLE_SCREEN_QUIT", "Quit" },
            { "TOP_BAR_DATE_FORMAT", "htt - MMM d yyyy" },
            { "TOP_BAR_KAKAPO_DETAILS", "Kakapo" },
            { "TOP_BAR_STAFF_DETAILS", "Staff" },
            { "TOP_BAR_RESEARCH_DETAILS", "Research" },
            { "TOP_BAR_TECHNOLOGY_DETAILS", "Technology" },
            { "TOP_BAR_FUNDING_DETAILS", "Funding" },
            { "KAKAPO_DETAILS_TITLE", "Kakapo Details" },
            { "STAFF_DETAILS_TITLE", "Staff Details" },
            { "RESEARCH_DETAILS_TITLE", "Research Details" },
            { "TECHNOLOGY_DETAILS_TITLE", "Technology Details" },
            { "FUNDING_DETAILS_TITLE", "Funding Details" }
        });

        // Scenes
        services
            .RegisterScene<ConservationTitleScene>()
            .RegisterScene<ConservationGameScene>()
            .RegisterScene<ConservationSimulationTestScene>();

        // Services
        services
            .AddScoped<ConservationGame>()
            .AddScoped<ConservationGameData>()
            .AddScoped(_ => new ConservationGameCamera(_.GetRequiredService<IInputManager>())
            {
                // TODO: Better way of applying defaults...
                Target = new Vector2(4500, 4000),
                Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
                Zoom = 0.10f
            })
            .AddScoped<IConservationGameInteractionService, ConservationGameInteractionService>()
            .AddScoped<IGameDateTimeProvider, GameDateTimeProvider>()
            .AddScoped<IGameCommandService, GameCommandService>()
            .AddScoped<IEventRoutingService, EventRoutingService>();

        // Commands
        services
            .AddTransient<IGameCommandHandler<SetScreenStateGameCommand>, SetScreenStateGameCommandHandler>()
            .AddTransient<IGameCommandHandler<SetInfoScreenGameCommand>, SetInfoScreenGameCommandHandler>();

        // Widgets
        services
            .AddScoped<TopBarWidget>()
            .AddScoped<KakapoDetailsUiSubScenePanelWidget>()
            .AddScoped<StaffDetailsUiSubScenePanelWidget>()
            .AddScoped<ResearchUiSubScenePanelWidget>()
            .AddScoped<TechnologyUiSubScenePanelWidget>()
            .AddScoped<FundingUiSubScenePanelWidget>()
            .AddScoped<InfoContextPanelWidget>();

        // Info panel sub widgets
        services
            .AddTransient<RegionSummaryInfoContextSubWidget>()
            .AddTransient<RegionInfoContextSubWidget>();

        return services;
    }
}