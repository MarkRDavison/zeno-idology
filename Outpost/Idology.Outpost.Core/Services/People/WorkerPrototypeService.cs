namespace Idology.Outpost.Core.Services.People;

public sealed class WorkerPrototypeService : PrototypeService<WorkerPrototype, Worker>
{
    public override Worker CreateEntity(WorkerPrototype prototype)
    {
        return new Worker
        {
            Id = Guid.NewGuid(),
            PrototypeId = prototype.Id,
            Class = prototype.Name,
            Inventory = prototype.Inventory.ToDictionary(_ => _.Key, _ => _.Value.Clone())
        };
    }
}
