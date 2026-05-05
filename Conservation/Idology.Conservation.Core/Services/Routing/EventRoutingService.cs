namespace Idology.Conservation.Core.Services.Routing;

internal sealed class EventRoutingService : IEventRoutingService
{
    public event EventHandler<SetMainScreenPanelPayload> SetMainScreenState = default!;
    public event EventHandler<PushInfoPanelPayload> PushInfoState = default!;
    public event EventHandler<PopInfoPanelPayload> PopInfoState = default!;
    public event EventHandler<OpenScreenPanelPayload> OpenScreenPanel = default!;
    public event EventHandler<CloseScreenPanelPayload> CloseScreenPanel = default!;

    public void InvokeSetMainScreenState(SetMainScreenPanelPayload payload)
    {
        SetMainScreenState?.Invoke(this, payload);
    }

    public void InvokePushInfoState(PushInfoPanelPayload payload)
    {
        PushInfoState?.Invoke(this, payload);
    }

    public void InvokePopInfoState(PopInfoPanelPayload payload)
    {
        PopInfoState?.Invoke(this, payload);
    }

    public void InvokeOpenScreenPanel(OpenScreenPanelPayload payload)
    {
        OpenScreenPanel?.Invoke(this, payload);
    }
    public void InvokeCloseScreenPanel(CloseScreenPanelPayload payload)
    {
        CloseScreenPanel?.Invoke(this, payload);
    }
}
