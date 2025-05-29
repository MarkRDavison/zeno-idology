namespace Idology.Outpost.Core.Services.People;

public sealed class PersonMovementService : IPersonMovementService
{
    private readonly GameData _gameData;

    public PersonMovementService(GameData gameData)
    {
        _gameData = gameData;
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
            g.TargetPosition = GameConstants.MusterPoint; // TODO: Waypoints to get through gate
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

        // TODO: Worker vs military etc
        if (_gameData.Town.TimeOfDay == TimeOfDay.Day && p.Class == "GUARD")
        {
            p.RequiresRemoval = true;
        }

        if (p.Class != "GUARD" && p.Mode == WorkerMode.TravellingToWork)
        {
            p.Mode = WorkerMode.Working;
            Console.WriteLine("{0} is now working", p.Class);
        }
        else if (p.Class != "GUARD" && p.Mode == WorkerMode.ReturningResources)
        {
            p.Mode = WorkerMode.TravellingToWork;
            Console.WriteLine("{0} has dropped off resources :) - now going back to work!", p.Class);

            p.TargetPosition = GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -1, 0) + Wiggle();
            p.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -8, 0) + Wiggle(3));
            p.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -12, 0) + Wiggle(4));
            p.Waypoints.Enqueue(GameConstants.HuntLocation + Wiggle(2));
        }
        if (p.Class != "GUARD" && p.Mode == WorkerMode.ReturningHome)
        {
            p.RequiresRemoval = true;
        }
    }

    private static Func<Person, bool> RequiresMovementFunc => _ => _.TargetPosition != _.Position;
    private static Func<Person, bool> IsGuard => _ => _.Class == "GUARD";
    private static Func<Person, bool> IsNotGuard => _ => _.Class != "GUARD";

    private static Vector2 Wiggle(int multiplier = 1) => new Vector2(
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier),
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier));

}
