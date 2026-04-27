namespace Idology.Conservation.Core.Services.Routing;

internal sealed class EventRoutingService : IEventRoutingService
{
    public event EventHandler<SetSubSceneGameCommand> SetSubScene = default!;

    public event EventHandler<PopSubSceneGameCommand> PopSubScene = default!;

    public void InvokeSetSubScene(SetSubSceneGameCommand command)
    {
        SetSubScene?.Invoke(this, command);
    }

    public void InvokePopSubScene(PopSubSceneGameCommand command)
    {
        PopSubScene?.Invoke(this, command);
    }
}
