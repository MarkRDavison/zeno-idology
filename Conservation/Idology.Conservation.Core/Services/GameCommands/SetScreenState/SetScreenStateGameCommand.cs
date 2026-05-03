namespace Idology.Conservation.Core.Services.GameCommands.SetScreenState;

public sealed class SetScreenStateGameCommand : IDeferredGameCommand
{
    public required ScreenState ScreenState { get; init; }
}
