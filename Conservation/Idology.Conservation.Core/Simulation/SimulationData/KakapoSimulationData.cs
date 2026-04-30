namespace Idology.Conservation.Core.Simulation.SimulationData;

public sealed record KakapoSimulationData(
    int KakapoId,
    int RegionId,
    Vector2 CurrentLocation);