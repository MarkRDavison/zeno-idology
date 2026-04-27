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

        for (int i = 0; i < 8; ++i)
        {
            var currentPanel = scrollableWidget.AddChild(new PanelWidget
            {
                Background = Color.Red,
                Border = Color.Magenta,
                BorderThickness = 4.0f,
                Layout =
                {
                    RequestedPadding = new LayoutEdges(8.0f),
                    Behave = BehaveFlags.HFill,
                    Contain = ContainFlags.Row,
                    Align = AlignFlags.Start,
                    ItemFlags = ItemFlags.VFixed
                }
            });

            currentPanel.AddChild(new LabelWidget
            {
                Foreground = Color.SkyBlue,
                TextContent = $"Scrollable #{i + 1}",
                FontSize = 64,
                Layout =
                {
                    RequestedMargin = new LayoutEdges(4.0f),
                    Behave = BehaveFlags.VCenter | BehaveFlags.Left,
                    RequestedSize = new LayoutVector(0, 64)
                }
            });
        }
    }
}
