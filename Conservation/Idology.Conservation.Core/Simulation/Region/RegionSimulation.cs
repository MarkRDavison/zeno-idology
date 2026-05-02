namespace Idology.Conservation.Core.Simulation.Region;

public sealed class RegionSimulation : ISimulationBase
{
    public int RegionId { get; }
    private const int Separation = 16;
    private readonly ConservationGameData _gameData;

    public RegionSimulation(
        int regionId,
        ConservationGameData gameData)
    {
        RegionId = regionId;
        _gameData = gameData;
    }

    public void Simulate(TimeSpan timespan)
    {
        // TODO: Cache these and have some busting mechanism.
        HashSet<Vector2> _validCells = [];

        var region = _gameData.Regions.First(_ => _.Id == RegionId);

        for (int y = 0; y < region.Height; ++y)
        {
            for (int x = 0; x < region.Width; ++x)
            {
                var tile = region.Tiles[y * region.Width + x];

                // TODO: Helper fxn for whether a tile is valid for a kakapo to be on???
                if (tile.TileType is TileType.Water or TileType.Beach or TileType.Unset)
                {
                    continue;
                }

                _validCells.Add(new Vector2(x, y));
            }
        }

        var kakapoToSimulate = _gameData.SimulatedKakapo.Where(_ => _.RegionId == RegionId).ToList();

        if (kakapoToSimulate.Count > 0 && KakapoDistribution.SpreadOut(kakapoToSimulate, _validCells, 1, Random.Shared, region.Width, region.Height, Separation, 0.25f))
        {
            foreach (var k in kakapoToSimulate)
            {
                var index = _gameData.SimulatedKakapo.FindIndex(_ => _.KakapoId == k.KakapoId);

                if (index >= 0)
                {
                    _gameData.SimulatedKakapo[index] = k;
                }
            }
        }
    }
}
