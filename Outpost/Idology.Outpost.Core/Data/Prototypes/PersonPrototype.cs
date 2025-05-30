namespace Idology.Outpost.Core.Data.Prototypes;

public sealed class PersonPrototype : IPrototype
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public float BaseWorkTime { get; set; }
    public Dictionary<string, AmountRange> Inventory { get; set; } = [];
    public Dictionary<string, int> WorkResult { get; set; } = [];
    public List<Vector2> WorkLocations { get; set; } = [];
}
