namespace Idology.Conservation.Core.Simulation.SimulationData;

public sealed class SoloSimulationRoot : ISimulationBase
{
    private readonly List<RegionSimulation> _regionSimulations = [];
    private readonly ResearchSimulation _researchSimulation;

    public SoloSimulationRoot(ResearchSimulation researchSimulation)
    {
        _researchSimulation = researchSimulation;
    }

    public void Simulate(TimeSpan timespan)
    {
        foreach (var rs in _regionSimulations)
        {
            rs.Simulate(timespan);
        }

        _researchSimulation.Simulate(timespan);
    }

    public List<RegionSimulation> RegionSimulations
    {
        set
        {
            _regionSimulations.Clear();
            _regionSimulations.AddRange(value);
        }
    }
}
