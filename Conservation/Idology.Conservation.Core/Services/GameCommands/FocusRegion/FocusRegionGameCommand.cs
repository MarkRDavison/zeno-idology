namespace Idology.Conservation.Core.Services.GameCommands.FocusRegion;

public sealed class FocusRegionGameCommand : IGameCommand
{
    public required int RegionId { get; init; }
}
