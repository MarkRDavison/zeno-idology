namespace Idology.Conservation.Core.Services.Routing;

internal sealed class EventRoutingService : IEventRoutingService
{
    public event EventHandler<SetScreenStateGameCommand> SetScreenState = default!;
    public event EventHandler<PushInfoPanelPayload> PushInfoState = default!;
    public event EventHandler<PopInfoPanelGameCommand> PopInfoState = default!;

    public void InvokeSetScreenState(SetScreenStateGameCommand command)
    {
        SetScreenState?.Invoke(this, command);
    }

    public void InvokePushInfoState(PushInfoPanelPayload command)
    {
        PushInfoState?.Invoke(this, command);
    }

    public void InvokePopInfoState(PopInfoPanelGameCommand command)
    {
        PopInfoState?.Invoke(this, command);
    }
}
