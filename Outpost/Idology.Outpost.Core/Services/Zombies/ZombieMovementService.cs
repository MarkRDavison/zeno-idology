using Idology.Outpost.Core.Data.Entities;

namespace Idology.Outpost.Core.Services.Zombies;

public sealed class ZombieMovementService : IZombieMovementService
{
    private readonly GameData _gameData;

    public ZombieMovementService(GameData gameData)
    {
        _gameData = gameData;
    }

    public void Update(float delta)
    {
        const float SPEED = 32.0f;
        foreach (var z in _gameData.Town.Zombies.Where(_ => _.TargetPosition != null))
        {
            var maxMovement = delta * SPEED;
            var offset = z.TargetPosition!.Value - z.Position;
            var distanceToTarget = offset.Length();

            if (distanceToTarget <= maxMovement)
            {
                z.TargetPosition = z.Position;
                HandleReachingTarget(z);

            }
            else
            {
                var direction = Vector2.Normalize(offset);

                z.Position += direction * maxMovement;
            }

            // TODO: Temp -> zombie reaches close enough to see wall/hears something
            if (z.Position.X > -GameConstants.TileSize * 7)
            {
                z.Mode = ZombieMode.Attacking;
                z.TargetPosition = new Vector2(-2 * GameConstants.PersonRadius * 1.5f, z.Position.Y);
                /* TODO: Assign the region that the zombie is attacking to it?
                 * So we round robin the guards to the regions so ideally at least one each
                 * Then ask for closest attacking zombie in the guards region
                 */
            }
        }
    }

    private void HandleReachingTarget(Zombie z)
    {
        if (z.Mode == ZombieMode.Wandering)
        {
            z.TargetPosition = null;
            z.IdleTime = 0;
        }
        else if (z.Mode == ZombieMode.Attacking)
        {

        }
    }
}
