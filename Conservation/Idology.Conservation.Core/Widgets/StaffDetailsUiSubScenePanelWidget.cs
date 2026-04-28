namespace Idology.Conservation.Core.Widgets;

public sealed class StaffDetailsUiSubScenePanelWidget : UiSubScenePanelWidget
{
    private readonly ConservationGameData _gameData;

    public StaffDetailsUiSubScenePanelWidget(
        ConservationGameData gameData,
        ITranslationService translationService
    ) : base(
        translationService)
    {
        _gameData = gameData;
    }

    public override string TitleTranslationKey => "STAFF_DETAILS_TITLE";

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets();

        foreach (var s in _gameData.StaffData)
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
