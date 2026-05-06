namespace Idology.Conservation.Core.Widgets;

internal sealed class InfoContextPanelWidget : PanelWidget, IDisposable
{
    private bool _disposedValue;
    internal const int Width = 512;
    internal const int Padding = 8;

    private readonly IEventRoutingService _eventRoutingService;
    private readonly IServiceProvider _serviceProvider;

    private Stack<PushInfoPanelPayload> _panelPayloadStack = [];

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
        Layout.RequestedSize = new LayoutVector(Width, 0.0f);
        Layout.RequestedPadding = new LayoutEdges(Padding);
        Layout.RequestedMargin = new LayoutEdges(8.0f);
        Layout.Behave = BehaveFlags.VFill | BehaveFlags.Right;
        Layout.Contain = ContainFlags.Flex;
        Layout.Align = AlignFlags.Start;

        _eventRoutingService.PushInfoState += OnPushInfoState;
        _eventRoutingService.PopInfoState += OnPopInfoState;
    }

    public override void Update(float delta)
    {
        UpdateChildren(delta);
        if (LayoutBoundsContainMousePosition())
        {
            // TODO: Better way of capturing actions...
            _ = InputManager.HandleActionIfInvoked(Constants.Action_Click);
            _ = InputManager.HandleActionIfInvoked(Constants.Action_Click_Start);
            _ = InputManager.HandleActionIfInvoked(Constants.Action_Click_Context);
        }
    }

    private void OnPopInfoState(object? sender, PopInfoPanelPayload e)
    {
        _panelPayloadStack.Pop();

        ClearChildren();

        if (_panelPayloadStack.Count > 0)
        {
            var top = _panelPayloadStack.Peek();

            PushInfoPanel(top);
        }
        else
        {
            Layout.Visibility = Visibility.Collapsed;
        }
    }

    private void PushInfoPanel(PushInfoPanelPayload payload)
    {
        var initializeSubWidgetFactory = new Dictionary<InfoState, Func<IServiceProvider, BaseWidget>>
        {
            {
                InfoState.RegionSummary,
                _ =>
                {
                    var widget = _.GetRequiredService<RegionSummaryInfoContextSubWidget>();
                    if (payload.Payload is RegionInfoPanelPayload p)
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
                    if (payload.Payload is RegionInfoPanelPayload p)
                    {
                        widget.SetRegionId(p.RegionId);
                    }
                    return widget;
                }
            },
            {
                InfoState.KakapoSummary,
                _ =>
                {
                    var widget = _.GetRequiredService<KakapoSummaryInfoContextSubWidget>();
                    if (payload.Payload is KakapoSummaryInfoPanelPayload p)
                    {
                        widget.SetKakapoId(p.KakapoId);
                    }
                    return widget;
                }
            }
        };

        if (initializeSubWidgetFactory.TryGetValue(payload.InfoState, out var newSubWidget))
        {
            AddGenericChild(newSubWidget(_serviceProvider));
        }
    }

    private void OnPushInfoState(object? sender, PushInfoPanelPayload e)
    {
        if (_panelPayloadStack.Count > 0 && _panelPayloadStack.Peek().InfoState == e.InfoState)
        {
            _panelPayloadStack.Pop();
        }

        _panelPayloadStack.Push(e);

        ClearChildren();

        Layout.Visibility = Visibility.Visible;

        PushInfoPanel(e);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _eventRoutingService.PopInfoState -= OnPopInfoState;
                _eventRoutingService.PushInfoState -= OnPushInfoState;
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