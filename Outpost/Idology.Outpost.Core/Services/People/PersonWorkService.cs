namespace Idology.Outpost.Core.Services.People;

public sealed class PersonWorkService : IPersonWorkService
{
    private readonly GameData _gameData;
    private readonly IPrototypeService<PersonPrototype, Person> _personPrototypeService;

    public PersonWorkService(
        GameData gameData,
        IPrototypeService<PersonPrototype, Person> personPrototypeService)
    {
        _gameData = gameData;
        _personPrototypeService = personPrototypeService;
    }

    public void Update(float _delta)
    {
        foreach (var p in _gameData.Town.People.Where(IsWorkingWorker))
        {
            p.ElapsedWork += _delta;
            var classThreshold = GetClassWorkThreshold(p.Class);
            if (p.ElapsedWork >= classThreshold)
            {
                p.ElapsedWork -= classThreshold;
                var (resource, amount) = GetClassWorkResult(p.Class);

                if (!p.Inventory.ContainsKey(resource))
                {
                    throw new InvalidOperationException();
                }

                var range = p.Inventory[resource];

                range.Current = Math.Min(range.Max, range.Current + amount);
                if (range.Current >= range.Max)
                {
                    p.Mode = WorkerMode.ReturningResources;
                    // TODO: Verify
                    /*
                     * Is this the right place to plan the walk back?
                     */
                    p.TargetPosition = GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -12, 0) + Wiggle(4);

                    p.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -8, 0) + Wiggle(3));
                    p.Waypoints.Enqueue(GameConstants.MusterPoint);
                }

            }
        }
    }

    private float GetClassWorkThreshold(string workerClass)
    {
        var prototype = _personPrototypeService.GetPrototype(workerClass);
        return prototype.BaseWorkTime;
    }

    private (string, int) GetClassWorkResult(string workerClass)
    {
        var prototype = _personPrototypeService.GetPrototype(workerClass);
        return (prototype.WorkResult.First().Key, prototype.WorkResult.First().Value); // TODO: BAD
    }

    public Func<Person, bool> IsWorkingWorker => _ =>
        _.Class != PrototypeConstants.Guard && // TODO: Non guard unit helper
        _.Mode == WorkerMode.Working;

    private static Vector2 Wiggle(int multiplier = 1) => new Vector2(
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier),
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier));
}
