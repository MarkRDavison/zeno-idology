using Idology.Conservation.Core.Services;

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
            { "TITLE_SCREEN_QUIT", "Quit" }
        });

        services
            .RegisterScene<ConservationTitleScene>()
            .RegisterScene<ConservationGameScene>();

        services
            .AddScoped<ConservationGame>()
            .AddScoped<ConservationGameRenderer>()
            .AddScoped<ConservationGameData>()
            .AddScoped<IConservationGameInteractionService, ConservationGameInteractionService>()
            .AddScoped<IGameDateTimeProvider, GameDateTimeProvider>();

        return services;
    }
}