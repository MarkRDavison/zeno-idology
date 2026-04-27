namespace Idology.Conservation.Core.Services.Routing;

public interface IEventRoutingService
{

    event EventHandler<SetSubSceneGameCommand> SetSubScene;

    event EventHandler<PopSubSceneGameCommand> PopSubScene;

    void InvokeSetSubScene(SetSubSceneGameCommand command);
    void InvokePopSubScene(PopSubSceneGameCommand command);
}
