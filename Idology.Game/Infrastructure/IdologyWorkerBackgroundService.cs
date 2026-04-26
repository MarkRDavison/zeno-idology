namespace Idology.Game.Infrastructure;

public abstract class IdologyWorkerBackgroundService : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    protected IdologyWorkerBackgroundService(
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

        await app.Init(scope.ServiceProvider.GetRequiredService<ITranslationService>()[WindowTitleTranslationKey]);

        LoadFonts(scope.ServiceProvider.GetRequiredService<IFontManager>());

        LoadTextures(scope.ServiceProvider.GetRequiredService<ITextureManager>());

        RegisterActions(scope.ServiceProvider.GetRequiredService<IInputManager>());

        BeforeStartInitialize(scope.ServiceProvider);

        Raylib_cs.Raylib.SetExitKey(Raylib_cs.KeyboardKey.Null);

        await app.Start(token);

        app.Stop();

        _hostApplicationLifetime.StopApplication();
    }

    protected abstract void RegisterActions(IInputManager inputManager);

    protected abstract void BeforeStartInitialize(IServiceProvider serviceProvider);

    protected virtual void LoadFonts(IFontManager fontManager)
    {

    }

    protected virtual void LoadTextures(ITextureManager textureManager)
    {
        if (Directory.Exists("Assets/Textures"))
        {
            foreach (var f in Directory.GetFiles("Assets/Textures"))
            {
                var path = Path.GetFullPath(f);
                var filename = Path.GetFileNameWithoutExtension(path);
                textureManager.LoadTexture(filename, path);
            }
        }
    }

    public virtual string WindowTitleTranslationKey => "WINDOW_TITLE";
}
