namespace Idology.Conservation.Core.Services.Routing;

public interface IEventRoutingService
{
    event EventHandler<SetMainScreenPanelPayload> SetMainScreenState;
    event EventHandler<PushInfoPanelPayload> PushInfoState;
    event EventHandler<PopInfoPanelPayload> PopInfoState;

    event EventHandler<OpenScreenPanelPayload> OpenScreenPanel;
    event EventHandler<CloseScreenPanelPayload> CloseScreenPanel;

    void InvokeSetMainScreenState(SetMainScreenPanelPayload payload);
    void InvokePushInfoState(PushInfoPanelPayload payload);
    void InvokePopInfoState(PopInfoPanelPayload payload);

    void InvokeOpenScreenPanel(OpenScreenPanelPayload payload);
    void InvokeCloseScreenPanel(CloseScreenPanelPayload payload);
}
