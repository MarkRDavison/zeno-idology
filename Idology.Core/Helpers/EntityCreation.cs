namespace Idology.Core.Helpers;

public static class EntityCreation
{
    public static void SummonWorkers(
        this World world,
        IPrototypeService<WorkerPrototype, WorkerComponent> prototypeService,
        int max)
    {
        var levelComponent = world.GetFirst<LevelComponent>();

        var foundAvailableSpot = true;
        while (max > 0 && foundAvailableSpot)
        {
            foundAvailableSpot = false;

            foreach (var e in world.GetWithAll<BuildingComponent>())
            {
                var b = e.Get<BuildingComponent>();
                if (max <= 0) { return; }
                foreach (var (id, r) in b.Accomodation.Where(_ => _.Value.Available > 0))
                {
                    foundAvailableSpot = true;
                    r.Current++;
                    world.CreateWorkerByAccomodation(
                        id,
                        prototypeService,
                        levelComponent);
                    max--;
                    break;
                }
            }
        }
    }

    private static void CreateWorkerByAccomodation(
        this World world,
        Guid accomodationId,
        IPrototypeService<WorkerPrototype, WorkerComponent> prototypeService,
        LevelComponent level)
    {
        var accomodationType = StringHash.ReverseHash(accomodationId);

        var worker = prototypeService.CreateEntity("Unemployed");

        if (level.TotalWorkers.TryGetValue(worker.PrototypeId, out var count))
        {
            level.TotalWorkers[worker.PrototypeId] += 1;
        }
        else
        {
            level.TotalWorkers.Add(worker.PrototypeId, 1);
        }

        world.Create(worker, new UnemployedComponent());
    }

    public static void CreateBuilding(
        this World world,
        IPrototypeService<BuildingPrototype, BuildingComponent> prototypeService,
        Vector2 tile,
        string prototypeName)
    {
        var prototype = prototypeService.GetPrototype(prototypeName);
        var building = prototypeService.CreateEntity(prototypeName);

        foreach (var levelEntity in world.GetWithAll<LevelComponent>())
        {
            var l = levelEntity.Get<LevelComponent>();

            foreach (var (id, amount) in prototype.ProvidedAccomodation)
            {
                if (!l.TotalAccomodation.TryAdd(id, amount))
                {
                    l.TotalAccomodation[id] += amount;
                }
            }

            foreach (var (id, amount) in prototype.ProvidedJobs)
            {
                if (!l.TotalJobs.TryAdd(id, amount))
                {
                    l.TotalJobs[id] += amount;
                }
            }

            var e = world.Create(
                new TransformComponent(tile * GameConstants.TileSize),
                new SpriteGraphicComponent
                {
                    Parts =
                    {
                        new SpriteGraphicItem(prototype.SheetName, prototype.SpriteName)
                    }
                },
                building);

            if (prototype.ProvidedJobs.Any())
            {
                var wc = new WorkplaceComponent
                {
                    // TODO: Assert prototype.Production is not null
                    Production = prototype.Production is null
                        ? new()
                        : prototype.Production.DeepClone(),
                    Job = prototype.ProvidedJobs.ToDictionary(_ => _.Key, _ => new AmountRange
                    {
                        Min = 0,
                        Current = 0,
                        Max = _.Value
                    })
                };

                e.Add(wc);
            }
        }
    }
}
