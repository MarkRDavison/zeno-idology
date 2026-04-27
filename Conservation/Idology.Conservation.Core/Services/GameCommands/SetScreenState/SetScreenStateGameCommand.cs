namespace Idology.Conservation.Core.Services.GameCommands.SetScreenState;

public sealed class SetScreenStateGameCommand : IGameCommand
{
    public required ScreenState ScreenState { get; init; }
}
