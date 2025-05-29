namespace Idology.Core.Prototypes;

public class BuildingPrototype : IPrototype
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SheetName { get; set; } = string.Empty;
    public string SpriteName { get; set; } = string.Empty;
    public Dictionary<Guid, int> ProvidedJobs { get; set; } = [];
    public Dictionary<Guid, int> ProvidedAccomodation { get; set; } = [];
    public Production? Production { get; set; }
}

public class Production : IDeepCloneable<Production>
{
    public float Time { get; set; }
    public Dictionary<string, int> Inputs { get; set; } = [];
    public Dictionary<string, ProductionRange> Outputs { get; set; } = [];

    public Production DeepClone()
    {
        return new Production
        {
            Time = Time,
            Inputs = Inputs.ToDictionary(_ => _.Key, _ => _.Value),
            Outputs = Outputs.ToDictionary(_ => _.Key, _ => _.Value with { }),
        };
    }
}