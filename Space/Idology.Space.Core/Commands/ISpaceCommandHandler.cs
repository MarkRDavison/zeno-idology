namespace Idology.Space.Core.Commands;


public interface ISpaceCommandHandlerBase
{

}

public interface ISpaceCommandHandler<TCommand> : ISpaceCommandHandlerBase
    where TCommand : class, ISpaceCommand
{
    void HandleCommand(TCommand command);
}
