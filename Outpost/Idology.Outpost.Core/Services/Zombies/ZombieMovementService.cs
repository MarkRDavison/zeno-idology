
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
        foreach (var z in _gameData.Town.Zombies.Where(_ => _.TargetPosition != null && _.Mode != ZombieMode.Attacking))
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
            if (z.Mode == ZombieMode.Wandering && z.Position.X > -GameConstants.TileSize * 7)
            {
                z.Mode = ZombieMode.Moving;
                /* TODO: Assign the region that the zombie is attacking to it?
                 * So we round robin the guards to the regions so ideally at least one each
                 * Then ask for closest attacking zombie in the guards region
                 */

                var (targetRegion, targetPosition) = GetTargetedRegionAndPosition(z.Position);
                z.TargetPosition = targetPosition;
                z.TargetRegion = targetRegion;
            }
        }
    }

    private (Vector2 targetRegion, Vector2 targetPosition) GetTargetedRegionAndPosition(Vector2 position)
    {
        var closestGuardLocationPerRegion = new List<(Vector2 RegionCoords, Vector2 GuardLocation, float Distance)>();

        foreach (var r in _gameData.Town.Regions)
        {
            var distancesWithIndex = Enumerable.Range(0, r.GuardPositions.Count).Select(_ => (_, (position - r.GuardPositions[_]).Length())).ToList();

            if (distancesWithIndex.Count == 0)
            {
                continue;
            }

            var closestDistance = distancesWithIndex.Select(_ => _.Item2).Min();

            var closestIndex = distancesWithIndex.First(_ => _.Item2 == closestDistance);

            closestGuardLocationPerRegion.Add((r.Coordinates, r.GuardPositions[closestIndex.Item1], closestDistance));
        }

        var closest = closestGuardLocationPerRegion.MinBy(_ => _.Distance);

        // TODO: This only works with guards on the left hand edge
        return (closest.RegionCoords, new Vector2(-2 * GameConstants.PersonRadius * 1.5f, closest.GuardLocation.Y));
    }

    private void HandleReachingTarget(Zombie z)
    {
        if (z.Mode == ZombieMode.Wandering)
        {
            z.TargetPosition = null;
            z.IdleTime = 0;
        }
        else if (z.Mode == ZombieMode.Moving)
        {
            z.TargetPosition = null;
            z.IdleTime = 0;
            z.Mode = ZombieMode.Attacking;
        }
    }
}
