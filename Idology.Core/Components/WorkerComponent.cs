namespace Idology.Core.Components;

public class UnemployedComponent
{

}
public class WorkerComponent : IEntity
{
    public Guid Id { get; set; }
    public Guid PrototypeId { get; set; }
    public Guid JobId { get; set; }
}
