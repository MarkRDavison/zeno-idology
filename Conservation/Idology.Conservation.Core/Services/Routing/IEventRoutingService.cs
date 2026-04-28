namespace Idology.Conservation.Core.Services.Routing;

public interface IEventRoutingService
{
    event EventHandler<SetScreenStateGameCommand> SetScreenState;
    event EventHandler<SetInfoScreenGameCommand> SetInfoState;

    void InvokeSetScreenState(SetScreenStateGameCommand command);
    void InvokeSetInfoState(SetInfoScreenGameCommand command);
}
