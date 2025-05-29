namespace Idology.Core.Prototypes;

public class WorkerPrototype : IPrototype
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public HashSet<Guid> JobTypes { get; set; } = [];
}
