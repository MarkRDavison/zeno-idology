namespace Idology.Conservation.Core.Services.GameCommands.InfoScreen;

public sealed record RegionInfoScreenPayload(int RegionId, bool IsSummary);
public sealed record KakapoInfoScreenPayload(int KakapoId);

public sealed class SetInfoScreenGameCommand : IDeferredGameCommand
{
    public required bool Open { get; init; }
    public required InfoState State { get; init; }
    public required object? Context { get; init; }
}
