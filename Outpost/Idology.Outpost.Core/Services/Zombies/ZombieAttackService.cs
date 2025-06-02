namespace Idology.Outpost.Core.Services.Zombies;

public sealed class ZombieAttackService : IZombieAttackService
{
    private readonly GameData _gameData;

    public ZombieAttackService(GameData gameData)
    {
        _gameData = gameData;
    }

    public void Update(float delta)
    {
        const float AttackThreshold = 4.0f;
        foreach (var z in _gameData.Town.Zombies.Where(IsAttacking))
        {
            var region = _gameData.Town.Regions.FirstOrDefault(_ => _.Coordinates == z.TargetRegion);
            if (region is null)
            {
                z.Mode = ZombieMode.Wandering;
                continue;
            }

            z.IdleTime += delta;
            if (z.IdleTime >= AttackThreshold)
            {
                z.IdleTime -= AttackThreshold;
                region.WallHealth -= z.Damage;
                Console.WriteLine("Region {0},{1} is down to {2}hp", region.Coordinates.X, region.Coordinates.Y, region.WallHealth);
            }
        }
    }

    private static Func<Zombie, bool> IsAttacking => _ => _.Mode == ZombieMode.Attacking;
}
