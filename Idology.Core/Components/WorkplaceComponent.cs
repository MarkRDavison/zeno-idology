namespace Idology.Core.Components;

public class WorkplaceComponent
{
    public Dictionary<Guid, AmountRange> Job { get; set; } = [];
    public Production Production { get; set; } = new();
    public float ProductionTime { get; set; }
}
