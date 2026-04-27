namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record FundingSubScenePayload();

internal sealed class FundingSubScene : SubScene<FundingSubScene, FundingSubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly IServiceProvider _serviceProvider;

    public FundingSubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        [FromKeyedServices(Constants.SubScene_FundingDetails)] IUserInterfaceRoot userInterfaceRoot,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _userInterfaceRoot = userInterfaceRoot;
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(FundingSubScenePayload? payload)
    {
        base.Init(payload);
        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - TopBarWidget.Height));

        {
            var staffDetails = _userInterfaceRoot.RootWidget.AddChild(_serviceProvider.GetRequiredService<FundingUiSubScenePanelWidget>());

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

        Raylib.DrawText("Funding", 32, 32, 48, Color.White);

        Raylib.EndMode2D();
    }
}
