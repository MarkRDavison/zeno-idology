namespace Idology.Conservation.Core.Widgets.InfoContextSubWidgets;

internal sealed class KakapoSummaryInfoContextSubWidget : BaseWidget
{
    private int _kakapoId;
    private readonly IConservationStateService _gameState;
    private readonly IGameCommandService _gameCommandService;

    public KakapoSummaryInfoContextSubWidget(
        IConservationStateService gameState,
        IInputManager inputManager,
        IUserInterfaceRoot userInterfaceRoot,
        IGameCommandService gameCommandService)
    {
        _gameState = gameState;
        InputManager = inputManager;
        UserInterfaceRoot = userInterfaceRoot;
        _gameCommandService = gameCommandService;
    }

    public void SetKakapoId(int kakapoId)
    {
        _kakapoId = kakapoId;
    }

    public KakapoModel Kakapo => _gameState.State.KakapoData.First(_ => _.Id == _kakapoId);
    public KakapoSimulationData SimulatedKakapo => _gameState.State.SimulatedKakapo.First(_ => _.KakapoId == _kakapoId);

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
            TextContent = Kakapo.Name,
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
            IconTextureName = "icon-x-24", // TODO: Constant...
            Layout = new LayoutItem
            {
                RequestedPadding = new LayoutEdges(2.0f),
                RequestedSize = new LayoutVector(32, 32),
                Contain = ContainFlags.Row,
                Behave = BehaveFlags.Top | BehaveFlags.Right
            }
        }).OnClick += (s, e) =>
        {
            _gameCommandService.EnqueueCommand(new PopInfoPanelGameCommand(InfoState.KakapoSummary));
        };
    }
}
