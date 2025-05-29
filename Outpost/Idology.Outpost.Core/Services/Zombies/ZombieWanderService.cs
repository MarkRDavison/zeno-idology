namespace Idology.Outpost.Core.Services.Zombies;

public sealed class ZombieWanderService : IZombieWanderService
{
    private readonly GameData _gameData;

    public ZombieWanderService(GameData gameData)
    {
        _gameData = gameData;
    }

    public void Wander(float delta)
    {
        const float WanderThreshold = 3.5f;
        const int range = (int)GameConstants.TileSize * 2;
        foreach (var z in _gameData.Town.Zombies)
        {
            if (z.TargetPosition is null)
            {
                z.IdleTime += delta;
                if (z.IdleTime > WanderThreshold)
                {
                    z.TargetPosition = z.Position + new Vector2(
                        Random.Shared.Next(-range * 1 / 4, range * 3 / 4),
                        Random.Shared.Next(-range / 2, range / 2));
                }
            }
        }
    }
}
