namespace Idology.Core.Services;

public interface IWorkerPrototypeService : IPrototypeService<WorkerPrototype, WorkerComponent>
{
    IList<WorkerPrototype> GetPrototypesThatCanPerformJob(IEnumerable<Guid> ids);
}
