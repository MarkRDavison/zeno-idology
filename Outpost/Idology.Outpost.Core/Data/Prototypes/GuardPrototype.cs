namespace Idology.Outpost.Core.Data.Prototypes;

public sealed class GuardPrototype : IPrototype
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public float BaseAttackTime { get; set; }
}
