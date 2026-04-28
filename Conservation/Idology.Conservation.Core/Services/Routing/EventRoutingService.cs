namespace Idology.Conservation.Core.Services.Routing;

internal sealed class EventRoutingService : IEventRoutingService
{
    public event EventHandler<SetScreenStateGameCommand> SetScreenState = default!;
    public event EventHandler<SetInfoScreenGameCommand> SetInfoState = default!;

    public void InvokeSetScreenState(SetScreenStateGameCommand command)
    {
        SetScreenState?.Invoke(this, command);
    }
    public void InvokeSetInfoState(SetInfoScreenGameCommand command)
    {
        SetInfoState?.Invoke(this, command);
    }
}
