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

        await app.Init("Idology Outpost");

        // TODO: Set working directory...

        var fontManager = scope.ServiceProvider.GetRequiredService<IFontManager>();
        fontManager.LoadFont("DEBUG", "Assets/Fonts/Kenney-Mini.ttf");
        fontManager.LoadFont("CALIBRIB", "Assets/Fonts/calibrib.ttf");

        var spriteManager = scope.ServiceProvider.GetRequiredService<ISpriteSheetManager>();

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


        }

        await app.Start(token);

        app.Stop();

        _hostApplicationLifetime.StopApplication();
    }
}