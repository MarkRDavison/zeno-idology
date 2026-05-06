using Idology.Conservation.Core.Services.GameCommands.OpenPanelState;
using Idology.Conservation.Core.Services.GameCommands.SetTimeMode;

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
        Layout.Gap = 4.0f;

        _gameCommandService = gameCommandService;
    }

    public override void PostConstructInit()
    {
        void AddSubWidgetButton(string translationKey, ScreenPanelState screenState)
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
                    Behave = BehaveFlags.Center | BehaveFlags.VFill,
                    RequestedSize = new LayoutVector(128, 0)
                }
            });

            button.OnClick += (s, e) => _gameCommandService.HandleCommand(new OpenPanelStateGameCommand(screenState, null));
        }

        AddSubWidgetButton("TOP_BAR_KAKAPO_DETAILS", ScreenPanelState.Kakapo);
        AddSubWidgetButton("TOP_BAR_STAFF_DETAILS", ScreenPanelState.Staff);
        AddSubWidgetButton("TOP_BAR_RESEARCH_DETAILS", ScreenPanelState.Research);
        AddSubWidgetButton("TOP_BAR_TECHNOLOGY_DETAILS", ScreenPanelState.Technology);
        AddSubWidgetButton("TOP_BAR_FUNDING_DETAILS", ScreenPanelState.Funding);

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
            BorderThickness = 2.0f
        });
        tcw.OnTimeModeChanged += (s, e) => _gameCommandService.HandleCommand(new SetTimeModeGameCommand(e));
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
