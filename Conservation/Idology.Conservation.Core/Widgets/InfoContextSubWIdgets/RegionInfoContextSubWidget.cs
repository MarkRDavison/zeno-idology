namespace Idology.Conservation.Core.Widgets.InfoContextSubWidgets;

internal sealed class RegionInfoContextSubWidget : BaseWidget
{
    private int _regionId;
    private readonly ConservationGameData _gameData;

    public RegionInfoContextSubWidget(
        ConservationGameData gameData,
        IInputManager inputManager,
        IUserInterfaceRoot userInterfaceRoot)
    {
        _gameData = gameData;
        InputManager = inputManager;
        UserInterfaceRoot = userInterfaceRoot;
    }

    public void SetRegionId(int regionId)
    {
        _regionId = regionId;
    }

    private RegionData Region => _gameData.Regions.ElementAt(_regionId);

    public override void PostConstructInit()
    {
        Layout.Behave = BehaveFlags.Fill;
        Layout.Contain = ContainFlags.Column;
        Layout.Align = AlignFlags.Start;

        AddChild(new LabelWidget
        {
            TextContent = Region.Name,
            Foreground = Color.White,
            Layout =
            {
                RequestedMargin = new LayoutEdges(4.0f, 0.0f, 0.0f, 0.0f),
                Behave = BehaveFlags.Left | BehaveFlags.Top,
                Align = AlignFlags.Start,
                RequestedSize = new LayoutVector(0, 36)
            }
        });
    }
}
