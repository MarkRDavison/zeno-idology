namespace Idology.Conservation.Core.Services.Routing;

public interface IEventRoutingService
{
    event EventHandler<SetScreenStateGameCommand> SetScreenState;

    void InvokeSetScreenState(SetScreenStateGameCommand command);
}
