namespace Idology.Conservation.Core.Services.GameCommands.SetScreenState;

public sealed record SetScreenStateGameCommand(
    ScreenState State
) : IDeferredGameCommand;

internal sealed class SetScreenStateGameCommandHandler : IDeferredGameCommandHandler<SetScreenStateGameCommand>
{
    private readonly IKakapoStateService _kakapoStateService;

    public SetScreenStateGameCommandHandler(IKakapoStateService kakapoStateService)
    {
        _kakapoStateService = kakapoStateService;
    }

    public bool CanHandleCommand(SetScreenStateGameCommand command) => true;

    public void HandleCommand(SetScreenStateGameCommand command)
    {
        switch (command.State)
        {
            case ScreenState.Kakapo:
                _kakapoStateService.OpenKakapoScreenState();
                break;
            default:
                Console.Error.WriteLine("UNHANDLED SET SCREEN STATE COMMAND");
                break;
        }
    }
}