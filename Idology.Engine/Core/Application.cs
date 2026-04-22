namespace Idology.Engine.Core;

// TODO: Application needs to be separated from the window and have its game loop called into by some external orchestrator
public class Application
{
    private readonly IServiceProvider _serviceProvider;

    private bool _running;

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
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.SetExitKey(0);
        Raylib.InitWindow(1440, 900, title);
        Raylib.InitAudioDevice();
        Raylib.SetWindowIcon(Raylib.LoadImage("Assets/Textures/icon.png"));

        ComponentBase.Services = _serviceProvider;

        await Task.CompletedTask;
    }

    public void Stop()
    {
        _running = false;
    }

    public Task Start(CancellationToken token)
    {
        _running = true;
        while (!Raylib.WindowShouldClose() && _running)
        {
            if (token.IsCancellationRequested)
            {
                break;
            }

            CurrentScene?.Update(Raylib.GetFrameTime());

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
