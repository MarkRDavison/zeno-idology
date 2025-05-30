namespace Idology.Outpost.Core.Services.People;

public sealed class PersonSpawnService : IPersonSpawnService
{
    private readonly GameData _gameData;
    private readonly IPrototypeService<PersonPrototype, Person> _personPrototypeService;

    public PersonSpawnService(
        GameData gameData,
        IPrototypeService<PersonPrototype, Person> personPrototypeService)
    {
        _gameData = gameData;
        _personPrototypeService = personPrototypeService;
    }

    public void HandleSunrise()
    {
        const int Hunters = 1;
        const int Lumberjacks = 2;

        _gameData.Town.People
            .AddRange(Enumerable.Range(0, Hunters)
            .Select(_ => SpawnHunterAtSunrise()));

        _gameData.Town.People
            .AddRange(Enumerable.Range(0, Lumberjacks)
            .Select(_ => SpawnLumberjackAtSunrise()));

    }

    public void HandleSunset()
    {
        var homeRegion = _gameData.Town.Regions.First(_ => _.Coordinates == new Vector2());

        // TODO: Temp only spawn allocated guards
        _gameData.Town.People
            .AddRange(Enumerable.Range(0, homeRegion.GuardPositions.Count)
            .Select(_ =>
            {
                var person = _personPrototypeService.CreateEntity(PrototypeConstants.Guard);
                person.Position = GameConstants.MusterPoint + new Vector2(15, 0) + Wiggle();
                person.TargetPosition = homeRegion.GuardPositions[_];
                return person;
            }));
    }

    public Person SpawnHunterAtSunrise()
    {
        var person = _personPrototypeService.CreateEntity(PrototypeConstants.Hunter);
        person.Mode = WorkerMode.TravellingToWork;
        person.Position = GameConstants.MusterPoint + new Vector2(15, 0) + Wiggle();
        person.TargetPosition = GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -1, 0) + Wiggle();

        person.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -8, 0) + Wiggle(3));
        person.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -12, 0) + Wiggle(4));
        person.Waypoints.Enqueue(GameConstants.HuntLocation + Wiggle(2));

        return person;
    }

    public Person SpawnLumberjackAtSunrise()
    {
        var person = _personPrototypeService.CreateEntity(PrototypeConstants.Lumberjack);
        person.Mode = WorkerMode.TravellingToWork;
        person.Position = GameConstants.MusterPoint + new Vector2(15, 0) + Wiggle();
        person.TargetPosition = GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -1, 0) + Wiggle();

        person.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -8, 1) + Wiggle(3));
        person.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -12, -1) + Wiggle(4));
        person.Waypoints.Enqueue(GameConstants.ForestLocation + Wiggle(2));

        return person;
    }

    private static Vector2 Wiggle(int multiplier = 1) => new Vector2(
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier),
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier));
}
