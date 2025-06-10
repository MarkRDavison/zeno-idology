namespace Idology.Outpost.Core.Services.People;

public sealed class PersonMovementService : IPersonMovementService
{
    private readonly GameData _gameData;
    private readonly IResourceService _resourceService;
    private readonly IPrototypeService<WorkerPrototype, Worker> _personPrototypeService;

    public PersonMovementService(
        GameData gameData,
        IResourceService resourceService,
        IPrototypeService<WorkerPrototype, Worker> personPrototypeService)
    {
        _gameData = gameData;
        _resourceService = resourceService;
        _personPrototypeService = personPrototypeService;
    }

    public void HandleSunrise()
    {
        foreach (var g in _gameData.Town.People.Where(IsGuard))
        {
            g.Waypoints.Clear();
            g.TargetPosition = GameConstants.MusterPoint;
        }
    }

    public void HandleSunset()
    {
        foreach (var g in _gameData.Town.People.Where(IsNotGuard))
        {
            g.Waypoints.Clear();
            // TODO: Helper to create waypoints home via gate etc
            g.TargetPosition = GameConstants.MusterPoint;
            g.Mode = WorkerMode.ReturningHome;
        }
    }

    public void Update(float delta)
    {
        const float SPEED = 96.0f;
        foreach (var p in _gameData.Town.People.Where(RequiresMovementFunc))
        {
            var maxMovement = delta * SPEED;
            var offset = p.TargetPosition - p.Position;
            var distanceToTarget = offset.Length();

            if (distanceToTarget <= maxMovement)
            {
                p.TargetPosition = p.Position;
                HandlePersonReachingTarget(p);
            }
            else
            {
                var direction = Vector2.Normalize(offset);

                p.Position += direction * maxMovement;
            }
        }

        _gameData.Town.People.RemoveAll(_ => _.RequiresRemoval);
    }

    private void HandlePersonReachingTarget(Person p)
    {
        if (p.Waypoints.Count > 0)
        {
            p.TargetPosition = p.Waypoints.Peek();
            p.Waypoints.Dequeue();
            return;
        }

        if (p is Guard g)
        {
            // TODO: Worker vs military etc
            if (_gameData.Town.TimeOfDay == TimeOfDay.Day)
            {
                g.RequiresRemoval = true;
            }
            else
            {
                g.Mode = WorkerMode.Working;
            }
        }
        else if (p is Worker w)
        {
            if (w.Mode == WorkerMode.TravellingToWork)
            {
                w.Mode = WorkerMode.Working;
            }
            else if (p.Mode == WorkerMode.ReturningResources)
            {
                foreach (var (r, range) in w.Inventory)
                {
                    _resourceService.IncreaseResources(new Dictionary<string, int>
                {
                    { r, range.Current }
                });
                    range.Current = 0;
                }

                w.Mode = WorkerMode.TravellingToWork;

                w.TargetPosition = GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -1, 0) + Wiggle();
                w.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -8, 0) + Wiggle(3));
                w.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -12, 0) + Wiggle(4));
                w.Waypoints.Enqueue(GetWorkLocation(w.Class) + Wiggle(2));
            }
            else if (w.Mode == WorkerMode.ReturningHome)
            {
                w.RequiresRemoval = true;
            }
        }

    }

    private static Func<Person, bool> RequiresMovementFunc => _ => _.TargetPosition != _.Position;
    private static Func<Person, bool> IsGuard => _ => _ is Guard;
    private static Func<Person, bool> IsNotGuard => _ => _ is Worker;

    private Vector2 GetWorkLocation(string workerClass)
    {
        var prototype = _personPrototypeService.GetPrototype(workerClass);
        return prototype.WorkLocations[Random.Shared.Next(0, prototype.WorkLocations.Count - 1)];
    }

    private static Vector2 Wiggle(int multiplier = 1) => new Vector2(
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier),
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier));

}
