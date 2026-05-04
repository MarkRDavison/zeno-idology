namespace Idology.Conservation.Core.Widgets.InfoContextSubWidgets;

internal sealed class RegionInfoContextSubWidget : BaseWidget
{
    private int _regionId;
    private readonly IConservationStateService _gameState;
    private readonly IGameCommandService _gameCommandService;

    public RegionInfoContextSubWidget(
        IConservationStateService gameState,
        IGameCommandService gameCommandService,
        IInputManager inputManager,
        IUserInterfaceRoot userInterfaceRoot)
    {
        _gameState = gameState;
        _gameCommandService = gameCommandService;
        InputManager = inputManager;
        UserInterfaceRoot = userInterfaceRoot;
    }

    public void SetRegionId(int regionId)
    {
        _regionId = regionId;
    }

    private RegionData Region => _gameState.State.Regions.ElementAt(_regionId);

    public override void PostConstructInit()
    {
        Layout.Behave = BehaveFlags.Fill;
        Layout.Contain = ContainFlags.Column;
        Layout.Align = AlignFlags.Start;

        var titleRow = AddChild(new PanelWidget
        {
            Layout =
            {
                Gap = 4.0f,
                Behave = BehaveFlags.Left | BehaveFlags.HFill | BehaveFlags.Top,
                Contain = ContainFlags.Row,
            }
        });

        titleRow.AddChild(new LabelWidget
        {
            TextContent = Region.Name,
            Foreground = Color.White,
            FontSize = 32,
            Layout =
            {
                Behave = BehaveFlags.Left | BehaveFlags.Top,
                Align = AlignFlags.Start,
                RequestedSize = new LayoutVector(300, 32)
            }
        });

        titleRow.AddChild(new SpacerWidget(ContainFlags.Row));

        titleRow.AddChild(new IconButtonWidget
        {
            Foreground = Color.White,
            IconTextureName = "icon-magnifier-24", // TODO: Constant...
            Layout = new LayoutItem
            {
                RequestedPadding = new LayoutEdges(2.0f),
                RequestedSize = new LayoutVector(32, 32),
                Contain = ContainFlags.Row,
                Behave = BehaveFlags.Top | BehaveFlags.Right
            }
        }).OnClick += (s, e) => _gameCommandService.HandleCommand(new FocusRegionGameCommand(_regionId));
    }
}
