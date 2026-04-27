namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record TechnologySubScenePayload();

internal sealed class TechnologySubScene : SubScene<TechnologySubScene, TechnologySubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly IServiceProvider _serviceProvider;

    public TechnologySubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        [FromKeyedServices(Constants.SubScene_TechnologyDetails)] IUserInterfaceRoot userInterfaceRoot,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _userInterfaceRoot = userInterfaceRoot;
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(TechnologySubScenePayload? payload)
    {
        base.Init(payload);
        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - TopBarWidget.Height));

        {
            var staffDetails = _userInterfaceRoot.RootWidget.AddChild(_serviceProvider.GetRequiredService<TechnologyUiSubScenePanelWidget>());

            staffDetails.Layout.Behave = BehaveFlags.Fill;
        }
    }

    public override void Update(float delta)
    {
        _userInterfaceRoot.Update(delta);
    }

    public override void Draw(Camera2D camera)
    {
        Raylib.BeginMode2D(camera);

        _userInterfaceRoot.RootWidget.Draw();

        Raylib.DrawText("Technology", 32, 32, 48, Color.White);

        Raylib.EndMode2D();
    }
}
