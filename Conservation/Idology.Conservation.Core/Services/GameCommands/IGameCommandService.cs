namespace Idology.Conservation.Core.Services.GameCommands;

public interface IGameCommandService
{
    bool HandleCommand<TCommand>(TCommand command) where TCommand : class, IGameCommand;
}
