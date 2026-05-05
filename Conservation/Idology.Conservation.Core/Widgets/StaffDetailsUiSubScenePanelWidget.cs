namespace Idology.Conservation.Core.Widgets;

public sealed class StaffDetailsUiSubScenePanelWidget : UiSubScenePanelWidget
{
    private readonly IConservationStateService _gameState;

    public StaffDetailsUiSubScenePanelWidget(
        IConservationStateService gameState,
        ITranslationService translationService,
        IGameCommandService gameCommandService
    ) : base(
        translationService,
        gameCommandService)
    {
        _gameState = gameState;
    }

    public override string TitleTranslationKey => "STAFF_DETAILS_TITLE";
    public override ScreenPanelState ScreenPanelState => ScreenPanelState.Staff;

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets();

        foreach (var s in _gameState.State.StaffData)
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
                Foreground = Color.White,
                TextContent = s.Name,
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
