namespace Idology.Core.Systems;

public sealed class CareerAllocatorSystem : WorldSystem
{
    private readonly IWorkerPrototypeService _workerPrototypeService;

    public CareerAllocatorSystem(IWorkerPrototypeService workerPrototypeService)
    {
        _workerPrototypeService = workerPrototypeService;
    }

    public override void Update(World world, float delta)
    {
        Entity levelEntity = world.GetWithAll<LevelComponent>().First();

        // TODO: Cache things??? or only run this 1/s
        // TODO: If someone loses there job, add the unallocated component back.
        // Then after a while give them the unemployed prototype
        // But they prioritize going back to the same career???

        var level = levelEntity.Get<LevelComponent>();

        var unemployed = world.GetWithAll<WorkerComponent, UnemployedComponent>();

        var workplaces = world.GetWithAll<WorkplaceComponent>();

        foreach (var worker in unemployed)
        {
            var workerComponent = worker.Get<WorkerComponent>();

            foreach (var workplace in workplaces)
            {
                var wc = workplace.Get<WorkplaceComponent>();

                var bc = workplace.Get<BuildingComponent>();

                var building = StringHash.ReverseHash(bc.PrototypeId);

                var availableJobTypes = wc.Job
                    .Where(_ => _.Value.Available > 0)
                    .Select(_ => _.Key)
                    .Distinct()
                    .ToList(); // TODO: Order by various priorities across and within workplaces

                if (!availableJobTypes.Any())
                {
                    continue;
                }

                var availablePrototypes = _workerPrototypeService.GetPrototypesThatCanPerformJob(availableJobTypes);

                if (!availablePrototypes.Any())
                {
                    continue;
                }

                if (!availablePrototypes.Any(_ => _.Id == workerComponent.PrototypeId))
                {
                    level.TotalWorkers[workerComponent.PrototypeId]--;
                    workerComponent.PrototypeId = availablePrototypes.First().Id;
                    if (!level.TotalWorkers.TryAdd(workerComponent.PrototypeId, 1))
                    {
                        level.TotalWorkers[workerComponent.PrototypeId]++;
                    }
                }

                worker.Remove<UnemployedComponent>();

                var newJobId = availableJobTypes.First();

                wc.Job[newJobId].Current++;
                workerComponent.JobId = newJobId; // TODO: Need to undo this???

                break;
            }
        }
    }
}
