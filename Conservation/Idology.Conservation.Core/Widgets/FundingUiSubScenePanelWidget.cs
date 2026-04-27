namespace Idology.Conservation.Core.Widgets;

public sealed class FundingUiSubScenePanelWidget : UiSubScenePanelWidget
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public FundingUiSubScenePanelWidget(
        IGameDateTimeProvider gameDateTimeProvider)
    {
        _gameDateTimeProvider = gameDateTimeProvider;

        Background = Color.Gray;
        Border = Color.DarkGray;
        BorderThickness = 2.0f;
        Layout.Behave = BehaveFlags.Fill;
        Layout.RequestedPadding = new LayoutEdges(4.0f);
        Layout.RequestedMargin = new LayoutEdges(4.0f);
    }
}
