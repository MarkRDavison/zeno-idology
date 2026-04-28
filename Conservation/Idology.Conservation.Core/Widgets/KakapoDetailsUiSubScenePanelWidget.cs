namespace Idology.Conservation.Core.Widgets;

public sealed class KakapoDetailsUiSubScenePanelWidget : UiSubScenePanelWidget
{
    private readonly ConservationGameData _gameData;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public KakapoDetailsUiSubScenePanelWidget(
        ConservationGameData gameData,
        ITranslationService translationService,
        IGameDateTimeProvider gameDateTimeProvider
    ) : base(
        translationService)
    {
        _gameData = gameData;
        _gameDateTimeProvider = gameDateTimeProvider;
    }

    public override string TitleTranslationKey => "KAKAPO_DETAILS_TITLE";

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets();

        foreach (var k in _gameData.KakapoData)
        {
            var currentPanel = scrollableWidget.AddChild(new PanelWidget
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                BorderThickness = 2.0f,
                Layout =
                {
                    RequestedPadding = new LayoutEdges(8.0f),
                    RequestedMargin = new LayoutEdges(0.0f, 0.0f, 0.0f, 2.0f), // TODO: REPLACE WITH PARENT GAP
                    Behave = BehaveFlags.HFill,
                    Contain = ContainFlags.Row,
                    Align = AlignFlags.Start,
                    ItemFlags = ItemFlags.VFixed
                }
            });

            currentPanel.AddChild(new LabelWidget
            {
                Foreground = Color.White,
                TextContent = k.Name,
                FontSize = 24,
                Layout =
                {
                    RequestedMargin = new LayoutEdges(4.0f),
                    Behave = BehaveFlags.VCenter | BehaveFlags.Left,
                    RequestedSize = new LayoutVector(0, 32)
                }
            });
        }
    }
}
