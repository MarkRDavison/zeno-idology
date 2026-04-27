namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record StaffDetialsSubScenePayload();

internal sealed class StaffDetialsSubScene : SubScene<StaffDetialsSubScene, StaffDetialsSubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly IServiceProvider _serviceProvider;

    public StaffDetialsSubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        [FromKeyedServices(Constants.SubScene_StaffDetails)] IUserInterfaceRoot userInterfaceRoot,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _userInterfaceRoot = userInterfaceRoot;
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(StaffDetialsSubScenePayload? payload)
    {
        base.Init(payload);
        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - TopBarWidget.Height));

        {
            var staffDetails = _userInterfaceRoot.RootWidget.AddChild(_serviceProvider.GetRequiredService<StaffDetailsUiSubScenePanelWidget>());

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

        Raylib.DrawText("Staff details", 32, 32, 48, Color.White);

        const int Padding = 4;

        var yPos = 32 + 48 + Padding;

        foreach (var kd in GameData.StaffData)
        {
            var summaryHeight = 0;

            Raylib.DrawText(kd.Name, 32 + Padding, yPos, 32, Color.Black);

            summaryHeight += 32;

            yPos += summaryHeight + Padding;
        }

        Raylib.EndMode2D();
    }
}
