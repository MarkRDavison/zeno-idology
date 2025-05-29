
namespace Idology.Core.Services;

public sealed class WorkerPrototypeService : PrototypeService<WorkerPrototype, WorkerComponent>, IWorkerPrototypeService
{
    public override WorkerComponent CreateEntity(WorkerPrototype prototype)
    {
        return new WorkerComponent
        {
            Id = Guid.NewGuid(),
            PrototypeId = prototype.Id
        };
    }

    public IList<WorkerPrototype> GetPrototypesThatCanPerformJob(IEnumerable<Guid> ids)
    {
        return GetPrototypes().Where(_ => ids.Any(id => _.JobTypes.Contains(id))).ToList();
    }
}
