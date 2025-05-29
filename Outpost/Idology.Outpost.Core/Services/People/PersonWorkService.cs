namespace Idology.Outpost.Core.Services.People;

public sealed class PersonWorkService : IPersonWorkService
{
    private readonly GameData _gameData;

    public PersonWorkService(GameData gameData)
    {
        _gameData = gameData;
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

    private static float GetClassWorkThreshold(string workerClass)
    {
        switch (workerClass)
        {
            case "HUNTER":
                return 2.5f;
            default:
                return 1.0f;
        }
    }

    public Func<Person, bool> IsWorkingWorker => _ =>
        _.Class != "GUARD" && // TODO: Non guard unit helper
        _.Mode == WorkerMode.Working;

    private static Vector2 Wiggle(int multiplier = 1) => new Vector2(
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier),
                    Random.Shared.Next(-15 * multiplier, +15 * multiplier));
}
