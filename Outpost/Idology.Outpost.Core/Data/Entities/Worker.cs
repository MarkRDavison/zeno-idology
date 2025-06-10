namespace Idology.Outpost.Core.Data.Entities;

public sealed class Worker : Person
{
    public Dictionary<string, AmountRange> Inventory { get; init; } = [];
    public Vector2? CurrentRegion { get; set; }
}
