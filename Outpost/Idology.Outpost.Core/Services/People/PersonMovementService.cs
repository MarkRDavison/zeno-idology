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
            g.TargetPosition = MusterPoint;
        }
    }

    public void HandleSunset()
    {
        var homeRegion = _gameData.Town.Regions.First(_ => _.Coordinates == new Vector2());

        // TODO: Temp only spawn allocated guards
        _gameData.Town.People
            .AddRange(Enumerable.Range(0, homeRegion.GuardPositions.Count)
            .Select(_ => new Person
            {
                Class = "GUARD",
                Position = MusterPoint + new Vector2(
                    15 + Random.Shared.Next(-15, +15),
                    Random.Shared.Next(-15, +15)),
                TargetPosition = homeRegion.GuardPositions[_]
            }));
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

        if (_gameData.Town.TimeOfDay == TimeOfDay.Day && p.Class == "GUARD")
        {
            p.RequiresRemoval = true;
        }
    }

    private static Func<Person, bool> RequiresMovementFunc => _ => _.TargetPosition != _.Position;
    private static Func<Person, bool> IsGuard => _ => _.Class == "GUARD";
    private static Vector2 MusterPoint => new(
        (int)(3 * GameConstants.TileSize),
        (int)(4 * GameConstants.TileSize));
}
