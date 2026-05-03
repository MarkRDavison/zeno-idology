namespace Idology.Conservation.Core.Widgets.InfoContextSubWidgets;

internal sealed class RegionSummaryInfoContextSubWidget : BaseWidget
{
    private int _regionId;
    private readonly ConservationGameData _gameData;
    private readonly IGameCommandService _gameCommandService;

    public RegionSummaryInfoContextSubWidget(
        ConservationGameData gameData,
        IInputManager inputManager,
        IUserInterfaceRoot userInterfaceRoot,
        IGameCommandService gameCommandService)
    {
        _gameData = gameData;
        InputManager = inputManager;
        UserInterfaceRoot = userInterfaceRoot;
        _gameCommandService = gameCommandService;
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

        var openRegionButton = titleRow.AddChild(new IconButtonWidget
        {
            Foreground = Color.White,
            IconTextureName = "icon-arrow-24", // TODO: Constant...
            Layout = new LayoutItem
            {
                RequestedPadding = new LayoutEdges(2.0f),
                RequestedSize = new LayoutVector(32, 32),
                Contain = ContainFlags.Row,
                Behave = BehaveFlags.Top | BehaveFlags.Right
            }
        });

        openRegionButton.OnClick += (s, e) =>
        {
            // TODO: Push and pop camera/transform matrices or position/zoom levels???
            _gameData.InteractionData.ScreenState = ScreenState.Region;
            _gameData.ActiveRegion = _gameData.Regions[_regionId];
            _gameData.InteractionData.DefaultScreenData.SelectedRegion = null;

            _gameCommandService.HandleCommand(new SetScreenStateGameCommand { ScreenState = ScreenState.Region, });

            _gameCommandService.HandleCommand(new SetInfoScreenGameCommand
            {
                Open = true,
                State = InfoState.Region,
                Context = new RegionInfoScreenPayload(_regionId, false)
            });
        };

        var closePanelButton = titleRow.AddChild(new IconButtonWidget
        {
            Foreground = Color.White,
            IconTextureName = "icon-x-24", // TODO: Constant...
            Layout = new LayoutItem
            {
                RequestedPadding = new LayoutEdges(2.0f),
                RequestedSize = new LayoutVector(32, 32),
                Contain = ContainFlags.Row,
                Behave = BehaveFlags.Top | BehaveFlags.Right
            }
        });

        closePanelButton.OnClick += (s, e) =>
        {
            _gameCommandService.HandleCommand(new SetInfoScreenGameCommand
            {
                Open = false,
                State = InfoState.Hidden,
                Context = null
            });
        };
    }
}
