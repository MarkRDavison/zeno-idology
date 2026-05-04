namespace Idology.Conservation.Core.Services.GameCommands.CloseScreenState;

public sealed record CloseScreenStateGameCommand(
    ScreenState ScreenState
) : IGameCommand;

internal sealed class CloseScreenStateGameCommandHandler : IGameCommandHandler<CloseScreenStateGameCommand>
{
    public bool HandleCommand(CloseScreenStateGameCommand command)
    {
        throw new NotImplementedException();
    }
}