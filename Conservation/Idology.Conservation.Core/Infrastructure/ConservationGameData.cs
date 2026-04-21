namespace Idology.Conservation.Core.Infrastructure;

public sealed class ConservationGameData
{
    public RegionData? ActiveRegion { get; set; }
    public List<RegionData> Regions { get; } = [];
    public ConservationInteractionData InteractionData { get; set; } = new();

    public List<KakapoData> KakapoData { get; } = [];
    public List<StaffData> StaffData { get; } = [];
}
