namespace Idology.Conservation.Core.Services.GameCommands.SetTimeMode;

public sealed record SetTimeModeGameCommand(
    TimeMode TimeMode
) : IGameCommand;

internal sealed class SetTimeModeGameCommandHandler : IGameCommandHandler<SetTimeModeGameCommand>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public SetTimeModeGameCommandHandler(IGameDateTimeProvider gameDateTimeProvider)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
    }

    public bool HandleCommand(SetTimeModeGameCommand command)
    {
        _gameDateTimeProvider.SetTimeMode(command.TimeMode);

        return true;
    }
}