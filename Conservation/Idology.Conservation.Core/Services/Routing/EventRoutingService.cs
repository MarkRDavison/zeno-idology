namespace Idology.Conservation.Core.Services.Routing;

internal sealed class EventRoutingService : IEventRoutingService
{
    public event EventHandler<SetScreenStateGameCommand> SetScreenState = default!;

    public void InvokeSetScreenState(SetScreenStateGameCommand command)
    {
        SetScreenState?.Invoke(this, command);
    }
}
