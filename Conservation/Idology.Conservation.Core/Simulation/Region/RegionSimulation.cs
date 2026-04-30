namespace Idology.Conservation.Core.Simulation.Region;

public sealed class RegionSimulation : ISimulationBase
{
    public int RegionId { get; }
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
        // TODO: Add id to region
        Console.WriteLine("Simulating Kakapo in region: {0}", _gameData.Regions.First(_ => _.Id == RegionId).Name);
        foreach (var simulatedKakapo in _gameData.SimulatedKakapo.Where(_ => _.RegionId == RegionId))
        {
            var kakapo = _gameData.KakapoData.First(_ => _.Id == simulatedKakapo.KakapoId);
            Console.WriteLine(" - {0}", kakapo.Name);
        }
    }
}
