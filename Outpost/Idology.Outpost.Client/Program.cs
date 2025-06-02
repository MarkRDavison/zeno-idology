using Idology.Outpost.Core.Data;

namespace Idology.Outpost.Client;

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
        // TODO: Raylib.SetTraceLogCallback -> redirect output to ILogger
        using var scope = _serviceScopeFactory.CreateScope();

        var app = scope.ServiceProvider.GetRequiredService<Application>();

        await app.Init("Idology Outpost");

        // TODO: Set working directory...

        var fontManager = scope.ServiceProvider.GetRequiredService<IFontManager>();
        fontManager.LoadFont("DEBUG", "Assets/Fonts/Kenney-Mini.ttf");
        fontManager.LoadFont("CALIBRIB", "Assets/Fonts/calibrib.ttf");

        var spriteManager = scope.ServiceProvider.GetRequiredService<ISpriteSheetManager>();

        var textureManager = scope.ServiceProvider.GetRequiredService<ITextureManager>();
        foreach (var f in Directory.GetFiles("Assets/Textures"))
        {
            var path = Path.GetFullPath(f);
            var filename = Path.GetFileNameWithoutExtension(path);
            textureManager.LoadTexture(filename, path);
        }

        var gameData = scope.ServiceProvider.GetRequiredService<GameData>();
        gameData.Resources.Add(ResourceConstants.Meat, new AmountRange { Min = 0, Current = 5, Max = 20 });
        gameData.Resources.Add(ResourceConstants.Wood, new AmountRange { Min = 0, Current = 100, Max = 200 });
        gameData.Resources.Add(ResourceConstants.Tools, new AmountRange { Min = 0, Current = 0, Max = 20 });
        gameData.Resources.Add(ResourceConstants.Wheat, new AmountRange { Min = 0, Current = 0, Max = 100 });
        gameData.Resources.Add(ResourceConstants.Stone, new AmountRange { Min = 0, Current = 0, Max = 100 });
        gameData.Resources.Add(ResourceConstants.Metal, new AmountRange { Min = 0, Current = 0, Max = 50 });

        // TODO: Config
        scope.ServiceProvider
            .GetRequiredService<ISceneService>()
            .SetScene<GameScene>();

        {
            var personPrototypeService = scope.ServiceProvider.GetRequiredService<IPrototypeService<PersonPrototype, Person>>();

            personPrototypeService.RegisterPrototype(
                PrototypeConstants.Hunter,
                new PersonPrototype
                {
                    Id = StringHash.Hash(PrototypeConstants.Hunter),
                    Name = PrototypeConstants.Hunter,
                    BaseWorkTime = 2.5f,
                    Inventory = { { ResourceConstants.Meat, new() { Max = 2 } } },
                    WorkResult = { { ResourceConstants.Meat, 1 } },
                    WorkLocations = [GameConstants.HuntLocation]
                });
            personPrototypeService.RegisterPrototype(
                PrototypeConstants.Lumberjack,
                new PersonPrototype
                {
                    Id = StringHash.Hash(PrototypeConstants.Lumberjack),
                    Name = PrototypeConstants.Lumberjack,
                    BaseWorkTime = 1.5f,
                    Inventory = { { ResourceConstants.Wood, new() { Max = 4 } } },
                    WorkResult = { { ResourceConstants.Wood, 1 } },
                    WorkLocations = [GameConstants.ForestLocation]
                });
            personPrototypeService.RegisterPrototype(
                PrototypeConstants.Guard,
                new PersonPrototype
                {
                    Id = StringHash.Hash(PrototypeConstants.Guard),
                    Name = PrototypeConstants.Guard
                });
        }

        {
            var zombiePrototypeService = scope.ServiceProvider.GetRequiredService<IPrototypeService<ZombiePrototype, Zombie>>();
            zombiePrototypeService.RegisterPrototype(
                PrototypeConstants.Zombie,
                new ZombiePrototype
                {
                    Id = StringHash.Hash(PrototypeConstants.Zombie),
                    Name = PrototypeConstants.Zombie,
                    Damage = 5
                });
        }

        Console.WriteLine("https://mrscauthd.github.io/Bliss/docs/getting-started.html");

        await app.Start(token);

        app.Stop();

        _hostApplicationLifetime.StopApplication();
    }
}