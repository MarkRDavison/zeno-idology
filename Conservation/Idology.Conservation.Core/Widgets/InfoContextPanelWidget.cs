namespace Idology.Conservation.Core.Widgets;

internal sealed class InfoContextPanelWidget : PanelWidget, IDisposable
{
    private bool _disposedValue;

    private readonly IEventRoutingService _eventRoutingService;

    public InfoContextPanelWidget(IEventRoutingService eventRoutingService)
    {
        _eventRoutingService = eventRoutingService;
    }

    public override void PostConstructInit()
    {
        Background = Color.Gray;
        Border = Color.DarkGray;
        BorderThickness = 2.0f;
        Layout.Visibility = Visibility.Collapsed;
        Layout.RequestedSize = new LayoutVector(512.0f, 0.0f);
        Layout.RequestedPadding = new LayoutEdges(8.0f);
        Layout.RequestedMargin = new LayoutEdges(8.0f);
        Layout.Behave = BehaveFlags.VFill | BehaveFlags.Right;
        Layout.Contain = ContainFlags.Column;
        Layout.ItemFlags = ItemFlags.HFixed;

        _eventRoutingService.SetInfoState += OnSetInfoState;
    }

    private void OnSetInfoState(object? sender, SetInfoScreenGameCommand e)
    {
        if (!e.Open)
        {
            Layout.Visibility = Visibility.Collapsed;
            return;
        }

        Layout.Visibility = Visibility.Visible;

        switch (e.State)
        {
            case InfoState.RegionSummary:
                break;
            case InfoState.Region:
                break;
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