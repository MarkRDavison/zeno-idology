namespace Idology.Conservation.Core.Widgets;

internal sealed class TopBarWidget : PanelWidget
{
    public const int Height = 48;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly ITranslationService _translationService;
    private readonly IGameCommandService _gameCommandService;

    public TopBarWidget(
        IGameDateTimeProvider gameDateTimeProvider,
        ITranslationService translationService,
        IGameCommandService gameCommandService)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _translationService = translationService;

        Background = Color.Gray;
        Border = Color.DarkGray;
        BorderThickness = 2.0f;
        Layout.RequestedPadding = new LayoutEdges(8.0f);
        Layout.RequestedSize = new LayoutVector(0, Height);
        Layout.Contain = ContainFlags.Row;
        Layout.Behave = BehaveFlags.HFill | BehaveFlags.Top;

        _gameCommandService = gameCommandService;
    }

    public override void PostConstructInit()
    {
        var isFirst = true;

        void AddSubWidgetButton(string translationKey, ScreenState screenState)
        {
            var button = AddChild(new TextButtonWidget
            {
                TextSize = 32,
                TextContent = _translationService[translationKey],
                Foreground = Color.White,
                Background = Color.Gray,
                Border = Color.DarkGray,
                BorderThickness = 2.0f,
                Layout = new LayoutItem
                {
                    // TODO: Replace this with a Gap/Spacing property on the parent...
                    RequestedMargin = new LayoutEdges(isFirst ? 0.0f : 4.0f, 0.0f, 0.0f, 0.0f),
                    Behave = BehaveFlags.Center | BehaveFlags.VFill,
                    RequestedSize = new LayoutVector(128, 0)
                }
            });

            button.OnClick += (s, e) => _gameCommandService.HandleCommand(new SetScreenStateGameCommand { ScreenState = screenState });

            isFirst = false;
        }

        AddSubWidgetButton("TOP_BAR_KAKAPO_DETAILS", ScreenState.Kakapo);
        AddSubWidgetButton("TOP_BAR_STAFF_DETAILS", ScreenState.Staff);
        AddSubWidgetButton("TOP_BAR_RESEARCH_DETAILS", ScreenState.Research);
        AddSubWidgetButton("TOP_BAR_TECHNOLOGY_DETAILS", ScreenState.Technology);
        AddSubWidgetButton("TOP_BAR_FUNDING_DETAILS", ScreenState.Funding);

        isFirst = false;
        // SPACER
        AddChild(new PanelWidget
        {
            Layout = new LayoutItem
            {
                RequestedSize = new LayoutVector(0, 0),
                Contain = ContainFlags.Row,
                Behave = BehaveFlags.Fill
            }
        });

        var tcw = AddChild(new TimeControlWidget(_gameDateTimeProvider)
        {
            Background = Color.Gray,
            Border = Color.DarkGray,
            BorderThickness = 2.0f,
            Layout = new LayoutItem
            {
                RequestedMargin = new LayoutEdges(0.0f, 0.0f, 2.0f, 0.0f),
                RequestedSize = new LayoutVector(156, 0),
                Contain = ContainFlags.Row,
                Behave = BehaveFlags.VFill
            }
        });
        tcw.OnSelectSpeed += (s, e) =>
        {
            Console.WriteLine("Setting speed to: {0}", e);
            _gameDateTimeProvider.SetTimeSpeed(e);
        };
        tcw.OnTogglePause += (s, e) =>
        {
            _gameDateTimeProvider.SetPauseState(!_gameDateTimeProvider.IsPaused);

            Console.WriteLine(_gameDateTimeProvider.IsPaused ? "PAUSED!" : "UN-PAUSED!");
        };
        AddChild(new DateTimeWidget(
            _gameDateTimeProvider,
            _translationService)
        {
            Background = Color.Gray,
            Border = Color.DarkGray,
            BorderThickness = 2.0f,
            Layout =
            {
                // Content???
                RequestedSize = new LayoutVector(384.0f, 0.0f),
                Align = AlignFlags.End,
                Behave = BehaveFlags.VFill
            }
        });
    }
}
