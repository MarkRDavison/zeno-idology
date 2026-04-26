namespace Idology.Conservation.Core.Widgets;

internal sealed class TopBarWidget : PanelWidget
{
    private const int Height = 48;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly ITranslationService _translationService;

    public TopBarWidget(
        IGameDateTimeProvider gameDateTimeProvider,
        ITranslationService translationService)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _translationService = translationService;

        Background = Color.Gray;
        Border = Color.DarkGray;
        BorderThickness = 2.0f;
        Layout.RequestedPadding = new LayoutEdges(4.0f);
        Layout.RequestedSize = new LayoutVector(0, Height);
    }

    public override void PostConstructInit()
    {
        AddChild(new PanelWidget
        {
            Background = Color.Gray,
            Border = Color.DarkGray,
            BorderThickness = 2.0f,
            Layout = new LayoutItem
            {
                RequestedMargin = new(),
                RequestedSize = new LayoutVector(128, 0),
                Contain = ContainFlags.Row,
                Behave = BehaveFlags.VFill
            }
        });
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
