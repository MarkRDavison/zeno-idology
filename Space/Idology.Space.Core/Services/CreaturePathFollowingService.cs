namespace Idology.Space.Core.Services;

public sealed class CreaturePathFollowingService : ICreaturePathFollowingService
{
    private readonly GameData _gameData;

    public CreaturePathFollowingService(GameData gameData)
    {
        _gameData = gameData;
    }

    public void Update(float delta)
    {
        if (_gameData.CurrentLevel is { } level)
        {
            foreach (var c in level.Creatures)
            {
                UpdateCreature(level, c, delta);
            }
        }
    }

    internal void UpdateCreature(LevelData level, Creature c, float delta)
    {
        if (c.TargetTile is not null)
        {

        }
        else if (c.Path.Count > 0)
        {
            c.TargetTile = c.Path.First();
            c.Path.RemoveAt(0);
            UpdateCreature(level, c, delta);
            return;
        }
    }
}
