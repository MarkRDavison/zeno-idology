namespace Idology.Conservation.Core.Services.GameCommands;

internal interface IGameCommandHandler<TCommand> where TCommand : class, IGameCommand
{
    bool HandleCommand(TCommand command);
}

internal interface IDeferredGameCommandHandler<TCommand> where TCommand : class, IDeferredGameCommand
{
    bool CanHandleCommand(TCommand command);
    void HandleCommand(TCommand command);
}
