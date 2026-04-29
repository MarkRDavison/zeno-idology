namespace Idology.Conservation.Core.Simulation;

public sealed class RegionSimulation : ISimulationBase
{
    // TODO: Id rather than name?
    private readonly string _name;

    public RegionSimulation(string name)
    {
        _name = name;
    }

    public void Simulate(TimeSpan timespan)
    {

    }
}
