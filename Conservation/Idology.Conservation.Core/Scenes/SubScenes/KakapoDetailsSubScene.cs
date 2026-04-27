namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record KakapoDetailsSubScenePayload();

internal sealed class KakapoDetailsSubScene : SubScene<KakapoDetailsSubScene, KakapoDetailsSubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IUserInterfaceRoot _userInterfaceRoot;
    private readonly IServiceProvider _serviceProvider;

    public KakapoDetailsSubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        [FromKeyedServices(Constants.SubScene_KakapoDetails)] IUserInterfaceRoot userInterfaceRoot,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _userInterfaceRoot = userInterfaceRoot;
        _serviceProvider = serviceProvider;
    }

    public override void Init(KakapoDetailsSubScenePayload? payload)
    {
        base.Init(payload);
        _userInterfaceRoot.SetBounds(new LayoutVector(Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - TopBarWidget.Height));

        {
            var kakapoDetails = _userInterfaceRoot.RootWidget.AddChild(_serviceProvider.GetRequiredService<KakapoDetailsUiSubScenePanelWidget>());

            kakapoDetails.Layout.Behave = BehaveFlags.Fill;
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

        Raylib.DrawText("Kakapo details", 32, 32, 48, Color.White);

        const int Padding = 4;

        var yPos = 32 + 48 + Padding;

        foreach (var kd in GameData.KakapoData)
        {
            var birdSummaryHeight = 0;

            var summaryLine = kd.Name;
            if (kd.Birth is not null)
            {
                summaryLine += $"\t({_gameDateTimeProvider.Date.Year - kd.Birth.Value.Year})";
            }
            Raylib.DrawText(summaryLine, 32 + Padding, yPos, 32, Color.Black);

            birdSummaryHeight += 32;

            yPos += birdSummaryHeight + Padding;
        }

        Raylib.EndMode2D();
    }
}
