namespace Idology.Outpost.Core.Services.Guards;

public sealed class GuardDefenceService : IGuardDefenceService
{
    private readonly GameData _gameData;

    public GuardDefenceService(GameData gameData)
    {
        _gameData = gameData;
    }

    public void Update(float delta)
    {
        const float AttackRate = 1.6f;
        const int GuardDamage = 4;
        bool zombieRemoved = false;
        var attackingZombies = _gameData.Town.Zombies.Where(IsAttacking);
        foreach (var g in _gameData.Town.Guards.Where(IsDefendingGuard))
        {
            g.ElapsedWork += delta;
            if (g.ElapsedWork >= AttackRate)
            {
                g.ElapsedWork = AttackRate;
                var zombiesAttackingRegion = attackingZombies.Where(_ => _.TargetRegion == g.CurrentRegion).ToList();

                if (zombiesAttackingRegion is { Count: 0 }) { continue; }

                /*
                 * PLAN
                 *
                 * Need to sort zombies based on distance from their original position
                 * (Allow 2 guards to attack 1 zombie?)
                 * Pick the closest one, and assign that as the target
                 * Move to be opposite them
                 * Attack until dead
                 * Repeat
                 * If no targets return to guard position
                 */

                if (zombiesAttackingRegion.FirstOrDefault(_ => _.Health > 0 && Math.Abs(_.Position.Y - g.Position.Y) < 2.0f) is { } zombie)
                {
                    zombie.Health -= GuardDamage;
                    g.ElapsedWork = 0.0f;
                    if (zombie.Health <= 0)
                    {
                        zombie.RequiresRemoval = true;
                        zombieRemoved = true;
                        Console.WriteLine("Zombie killed!");
                    }
                }
            }
        }

        if (zombieRemoved)
        {
            _gameData.Town.Zombies.RemoveAll(_ => _.RequiresRemoval);
        }
    }

    private static Func<Zombie, bool> IsAttacking => _ => _.Mode == ZombieMode.Attacking && _.TargetRegion is not null;
    private static Func<Guard, bool> IsDefendingGuard => _ => _.Mode == WorkerMode.Working && _.CurrentRegion is not null;
}
