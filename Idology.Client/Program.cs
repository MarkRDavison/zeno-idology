using Idology.Core.Scenes;

namespace Idology.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateDefaultApp(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateDefaultApp(string[] args) => Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services
                    .AddHostedService<Worker>()
                    .AddEngine()
                    .AddClient();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
}

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(
        IHostApplicationLifetime hostApplicationLifetime,
        IServiceScopeFactory serviceScopeFactory)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        Console.WriteLine("TODO: Powered by Raylib");

        using var scope = _serviceScopeFactory.CreateScope();

        var app = scope.ServiceProvider.GetRequiredService<Application>();

        await app.Init("Idology");

        var fontManager = scope.ServiceProvider.GetRequiredService<IFontManager>();
        fontManager.LoadFont("DEBUG", "Assets/Fonts/Kenney-Mini.ttf");
        fontManager.LoadFont("CALIBRIB", "Assets/Fonts/calibrib.ttf");

        var spriteManager = scope.ServiceProvider.GetRequiredService<ISpriteSheetManager>();
        spriteManager.LoadSpriteSheet(ResourceConstants.MedievalSpriteSheet, "Assets/Textures/medievalRTS_spritesheet@2.xml");
        spriteManager.LoadSpriteSheet(ResourceConstants.UiSpriteSheet, "Assets/Textures/uipack_rpg_sheet.xml");
        spriteManager.LoadSpriteSheet(ResourceConstants.CustomSpriteSheet, "Assets/Textures/CustomSpriteSheet.xml");

        // TODO: Config
        scope.ServiceProvider.GetRequiredService<ISceneService>().SetScene<GameScene>();

        await app.Start(token);

        app.Stop();

        _hostApplicationLifetime.StopApplication();
    }
}