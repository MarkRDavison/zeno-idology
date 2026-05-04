namespace Idology.Conservation.Core.Simulation.Region;

public sealed class RegionSimulation : ISimulationBase
{
    public int RegionId { get; }
    private const int Separation = 16;
    private readonly IConservationStateService _gameState;

    public RegionSimulation(
        int regionId,
        IConservationStateService gameState)
    {
        RegionId = regionId;
        _gameState = gameState;
    }

    public void Simulate(TimeSpan timespan)
    {
        // TODO: Cache these and have some busting mechanism.
        HashSet<Vector2> _validCells = [];

        var region = _gameState.State.Regions.First(_ => _.Id == RegionId);

        for (int y = 0; y < region.Height; ++y)
        {
            for (int x = 0; x < region.Width; ++x)
            {
                var tile = region.Tiles[y * region.Width + x];

                // TODO: Helper fxn for whether a tile is valid for a kakapo to be on???
                // Maybe exclude coast tiles? Or just set them as cliff/beach
                if (tile.TileType is TileType.Water or TileType.Beach or TileType.Unset or TileType.Cliff)
                {
                    continue;
                }

                _validCells.Add(new Vector2(x, y));
            }
        }

        var kakapoToSimulate = _gameState.State.SimulatedKakapo.Where(_ => _.RegionId == RegionId).ToList();

        if (kakapoToSimulate.Count > 0 && KakapoDistribution.SpreadOut(kakapoToSimulate, _validCells, 1, Random.Shared, region.Width, region.Height, Separation, 0.25f))
        {
            _gameState.SetState(_ => _.WithUpdatedKakapoSimulations(kakapoToSimulate));
        }
    }
}
