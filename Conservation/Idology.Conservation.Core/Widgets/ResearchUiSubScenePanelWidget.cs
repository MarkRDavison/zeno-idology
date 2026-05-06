namespace Idology.Conservation.Core.Widgets;

public sealed class ResearchUiSubScenePanelWidget : UiSubScenePanelWidget
{
    private readonly IConservationStateService _gameState;

    public ResearchUiSubScenePanelWidget(
        ITranslationService translationService,
        IGameCommandService gameCommandService,
        IConservationStateService gameState
    ) : base(
        translationService,
        gameCommandService)
    {
        _gameState = gameState;
    }

    public override string TitleTranslationKey => "RESEARCH_DETAILS_TITLE";
    public override ScreenPanelState ScreenPanelState => ScreenPanelState.Research;

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets();

        foreach (var r in _gameState.State.ResearchData)
        {
            var currentPanel = scrollableWidget.AddChild(new PanelWidget
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                BorderThickness = 2.0f,
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
                Foreground = AllPreReqsDone(_gameState.State.ResearchData, r)
                    ? (r.Cost <= r.Researched
                        ? Color.Green
                        : Color.White)
                    : Color.Red,
                TextContent = r.Name,
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

    private static bool AllPreReqsDone(IReadOnlyList<ResearchData> allResearchData, ResearchData r)
    {
        return r.Prerequisites.All(p => allResearchData.FirstOrDefault(r => r.Id == p) is { } pre && pre.Cost <= pre.Researched);
    }
}
