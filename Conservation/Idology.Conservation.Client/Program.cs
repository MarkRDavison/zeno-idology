namespace Idology.Conservation.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateDefaultApp(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateDefaultApp(string[] args) => Host
        .CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services
                .AddHostedService<Worker>()
                .AddEngine()
                .AddUserInterface()
                .AddClient();
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
}

// TODO: Move this to framework and accept On init overrides etc
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

        RaylibLogger.Initialize(scope.ServiceProvider.GetRequiredService<ILoggerFactory>());

        var app = scope.ServiceProvider.GetRequiredService<Application>();

        await app.Init(scope.ServiceProvider.GetRequiredService<ITranslationService>()["WINDOW_TITLE"]);

        // TODO: Set working directory...

        var fontManager = scope.ServiceProvider.GetRequiredService<IFontManager>();
        fontManager.LoadFont("DEBUG", "Assets/Fonts/Kenney-Mini.ttf");
        fontManager.LoadFont("CALIBRIB", "Assets/Fonts/calibrib.ttf");

        var spriteManager = scope.ServiceProvider.GetRequiredService<ISpriteSheetManager>();

        var textureManager = scope.ServiceProvider.GetRequiredService<ITextureManager>();
        if (Directory.Exists("Assets/Textures"))
        {
            foreach (var f in Directory.GetFiles("Assets/Textures"))
            {
                var path = Path.GetFullPath(f);
                var filename = Path.GetFileNameWithoutExtension(path);
                textureManager.LoadTexture(filename, path);
            }
        }

        var data = scope.ServiceProvider.GetRequiredService<ConservationGameData>();

        Raylib_cs.Raylib.SetExitKey(Raylib_cs.KeyboardKey.Null);

        // TODO: Config
        scope.ServiceProvider
            .GetRequiredService<ISceneService>()
            .SetScene<ConservationTitleScene>(null);

        await app.Start(token);

        app.Stop();

        _hostApplicationLifetime.StopApplication();
    }
}