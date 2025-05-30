namespace Idology.Engine.Core;

public class Application
{
    private readonly IServiceProvider _serviceProvider;

    public Application(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Init(string title)
    {
        //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_TOPMOST);
        //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_MAXIMIZED);
        //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_UNDECORATED);
        //var monitor = Raylib.GetCurrentMonitor();
        //var width = Raylib.GetMonitorWidth(monitor);
        //var height = Raylib.GetMonitorHeight(monitor);

        //Raylib.SetWindowSize(width, height);

        Raylib.SetConfigFlags(ConfigFlags.VSyncHint);
        Raylib.SetExitKey(0);
        Raylib.InitWindow(1440, 900, title);
        Raylib.InitAudioDevice();
        Raylib.SetWindowIcon(Raylib.LoadImage("Assets/Textures/icon.png"));

        ComponentBase.Services = _serviceProvider;

        await Task.CompletedTask;
    }

    public void Stop()
    {
        Raylib.CloseWindow();
    }

    public Task Start(CancellationToken token)
    {
        while (!Raylib.WindowShouldClose()) // Detect window close button or ESC key
        {
            if (token.IsCancellationRequested)
            {
                break;
            }

            CurrentScene?.Update(1.0f / 60.0f);// Raylib.GetFrameTime());

            CurrentScene?.Draw();
        }

        return Task.CompletedTask;
    }

    public void SetScene(Scene? scene)
    {
        CurrentScene = scene;
    }

    public Scene? CurrentScene { get; private set; }

}
