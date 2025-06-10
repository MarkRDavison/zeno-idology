namespace Idology.Outpost.Core.Services.People;

public sealed class PersonWorkService : IPersonWorkService
{
    private readonly GameData _gameData;
    private readonly IPrototypeService<WorkerPrototype, Worker> _personPrototypeService;

    public PersonWorkService(
        GameData gameData,
        IPrototypeService<WorkerPrototype, Worker> personPrototypeService)
    {
        _gameData = gameData;
        _personPrototypeService = personPrototypeService;
    }

    public void Update(float _delta)
    {
        foreach (var w in _gameData.Town.Workers.Where(IsWorkingWorker))
        {
            w.ElapsedWork += _delta;
            var classThreshold = GetClassWorkThreshold(w.Class);
            if (w.ElapsedWork >= classThreshold)
            {
                w.ElapsedWork -= classThreshold;
                var (resource, amount) = GetClassWorkResult(w.Class);

                if (!w.Inventory.TryGetValue(resource, out AmountRange? range))
                {
                    throw new InvalidOperationException(); // TODO: Validate this at prototype loading stage
                }

                range.Current = Math.Min(range.Max, range.Current + amount);
                if (range.Current >= range.Max)
                {
                    w.Mode = WorkerMode.ReturningResources;
                    // TODO: Verify
                    /*
                     * Is this the right place to plan the walk back?
                     */
                    w.TargetPosition = GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -12, 0) + Wiggle(4);

                    w.Waypoints.Enqueue(GameConstants.MusterPoint + new Vector2(GameConstants.TileSize * -8, 0) + Wiggle(3));
                    w.Waypoints.Enqueue(GameConstants.MusterPoint);
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

    private static Func<Worker, bool> IsWorkingWorker => _ => _.Mode == WorkerMode.Working;

    private static Vector2 Wiggle(int multiplier = 1) => new Vector2(
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier),
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier));
}
