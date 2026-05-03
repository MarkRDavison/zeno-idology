namespace Idology.Conservation.Core.Widgets;

internal sealed class InfoContextPanelWidget : PanelWidget, IDisposable
{
    private bool _disposedValue;
    internal const int Width = 512;
    internal const int Padding = 8;

    private readonly IEventRoutingService _eventRoutingService;
    private readonly IServiceProvider _serviceProvider;

    public InfoContextPanelWidget(
        IEventRoutingService eventRoutingService,
        IServiceProvider serviceProvider)
    {
        _eventRoutingService = eventRoutingService;
        _serviceProvider = serviceProvider;
    }

    public override void PostConstructInit()
    {
        Background = Color.Gray;
        Border = Color.DarkGray;
        BorderThickness = 2.0f;
        Layout.Visibility = Visibility.Collapsed;
        Layout.RequestedSize = new LayoutVector(Width, 0.0f); // TODO: Constant
        Layout.RequestedPadding = new LayoutEdges(Padding);
        Layout.RequestedMargin = new LayoutEdges(8.0f);
        Layout.Behave = BehaveFlags.VFill | BehaveFlags.Right;
        Layout.Contain = ContainFlags.Flex;
        Layout.Align = AlignFlags.Start;

        _eventRoutingService.SetInfoState += OnSetInfoState;
    }

    private void OnSetInfoState(object? sender, SetInfoScreenGameCommand e)
    {
        ClearChildren();

        if (!e.Open)
        {
            Layout.Visibility = Visibility.Collapsed;
            return;
        }

        Layout.Visibility = Visibility.Visible;

        var initializeSubWidgetFactory = new Dictionary<InfoState, Func<IServiceProvider, BaseWidget>>
        {
            {
                InfoState.RegionSummary,
                _ =>
                {
                    var widget = _.GetRequiredService<RegionSummaryInfoContextSubWidget>();
                    if (e.Context is RegionInfoScreenPayload p)
                    {
                        widget.SetRegionId(p.RegionId);
                    }
                    return widget;
                }
            },
            {
                InfoState.Region,
                _ =>
                {
                    var widget = _.GetRequiredService<RegionInfoContextSubWidget>();
                    if (e.Context is RegionInfoScreenPayload p)
                    {
                        widget.SetRegionId(p.RegionId);
                    }
                    return widget;
                }
            }
        };

        if (initializeSubWidgetFactory.TryGetValue(e.State, out var newSubWidget))
        {
            AddGenericChild(newSubWidget(_serviceProvider));
        }
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _eventRoutingService.SetInfoState -= OnSetInfoState;
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}