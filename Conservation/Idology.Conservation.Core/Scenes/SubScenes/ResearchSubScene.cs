namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record ResearchSubScenePayload();

internal sealed class ResearchSubScene : SubScene<ResearchSubScene, ResearchSubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly IServiceProvider _serviceProvider;

    public ResearchSubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        [FromKeyedServices(Constants.SubScene_ResearchDetails)] IUserInterfaceRoot userInterfaceRoot,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _userInterfaceRoot = userInterfaceRoot;
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(ResearchSubScenePayload? payload)
    {
        base.Init(payload);
        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - TopBarWidget.Height));

        {
            var staffDetails = _userInterfaceRoot.RootWidget.AddChild(_serviceProvider.GetRequiredService<ResearchUiSubScenePanelWidget>());

            staffDetails.Layout.Behave = BehaveFlags.Fill;
        }
    }

    public override void OnWindowResize(int width, int height)
    {
        _userInterfaceRoot.SetBounds(new LayoutVector(width, height - TopBarWidget.Height));
    }

    public override void Update(float delta)
    {
        _userInterfaceRoot.Update(delta);
    }

    public override void Draw(Camera2D camera)
    {
        Raylib.BeginMode2D(camera);

        _userInterfaceRoot.RootWidget.Draw();

        Raylib.EndMode2D();
    }
}
