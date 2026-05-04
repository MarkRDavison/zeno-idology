namespace Idology.Conservation.Core.Services.Routing;

public interface IEventRoutingService
{
    event EventHandler<SetScreenStateGameCommand> SetScreenState;
    event EventHandler<PushInfoPanelPayload> PushInfoState;
    event EventHandler<PopInfoPanelGameCommand> PopInfoState;

    void InvokeSetScreenState(SetScreenStateGameCommand command);
    void InvokePushInfoState(PushInfoPanelPayload command);
    void InvokePopInfoState(PopInfoPanelGameCommand command);
}
