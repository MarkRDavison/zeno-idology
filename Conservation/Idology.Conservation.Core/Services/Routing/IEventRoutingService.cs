namespace Idology.Conservation.Core.Services.Routing;

public interface IEventRoutingService
{
    event EventHandler<SetScreenStateGameCommand> SetScreenState;
    event EventHandler<PushInfoPanelGameCommand> PushInfoState;
    event EventHandler<PopInfoPanelGameCommand> PopInfoState;

    void InvokeSetScreenState(SetScreenStateGameCommand command);
    void InvokePushInfoState(PushInfoPanelGameCommand command);
    void InvokePopInfoState(PopInfoPanelGameCommand command);
}
