namespace Idology.Conservation.Core.Infrastructure;

public record ConservationGameData(
    ConservationInteractionData InteractionData,
    RegionData? ActiveRegion, // TODO: Should this just be a getter? Based on active region?
    IReadOnlyList<RegionData> Regions,
    IReadOnlyList<KakapoModel> KakapoData,
    IReadOnlyList<StaffData> StaffData,
    IReadOnlyList<RegionSimulation> RegionSimulations,
    IReadOnlyList<KakapoSimulationData> SimulatedKakapo);