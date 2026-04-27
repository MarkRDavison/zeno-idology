namespace Idology.Conservation.Core.Services.GameCommands;

internal interface IGameCommandHandler<TCommand> where TCommand : class, IGameCommand
{
    bool HandleCommand(TCommand command);
}
