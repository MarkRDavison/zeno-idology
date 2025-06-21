namespace Idology.Space.Core.Commands;


public interface ISpaceCommandHandlerBase
{

}

public interface ISpaceCommandHandler<TCommand> : ISpaceCommandHandlerBase
    where TCommand : class, ISpaceCommand
{
    void HandleCommand(TCommand command);
}

public interface ISpaceCommandHandler<TCommand, TResult> : ISpaceCommandHandlerBase
    where TCommand : class, ISpaceCommand
{
    TResult HandleCommand(TCommand command);
}