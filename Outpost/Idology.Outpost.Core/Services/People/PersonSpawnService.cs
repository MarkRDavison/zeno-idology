namespace Idology.Outpost.Core.Services.People;

public sealed class PersonSpawnService : IPersonSpawnService
{
    private readonly GameData _gameData;

    public PersonSpawnService(GameData gameData)
    {
        _gameData = gameData;
    }

    public void HandleSunrise()
    {
        const int Hunters = 4;

        _gameData.Town.People
            .AddRange(Enumerable.Range(0, Hunters)
            .Select(_ =>
            {
                var person = new Person
                {
                    Mode = WorkerMode.TravellingToWork,
                    Class = "HUNTER",
                    Position = GameConstants.MusterPoint + new Vector2(
                    15 + Random.Shared.Next(-15, +15),
                    Random.Shared.Next(-15, +15)),
                    TargetPosition = GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -1, 0) + Wiggle()
                };

                person.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -8, 0) + Wiggle(3));
                person.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -12, 0) + Wiggle(4));
                person.Waypoints.Enqueue(GameConstants.HuntLocation + Wiggle(2));

                return person;
            }));

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
                Position = GameConstants.MusterPoint + new Vector2(
                    15 + Random.Shared.Next(-15, +15),
                    Random.Shared.Next(-15, +15)),
                TargetPosition = homeRegion.GuardPositions[_]
            }));
    }

    private static Vector2 Wiggle(int multiplier = 1) => new Vector2(
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier),
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier));
}
