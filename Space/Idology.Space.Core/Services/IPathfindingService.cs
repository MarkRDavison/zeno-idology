namespace Idology.Space.Core.Services;

public interface IPathfindingService
{
    public Vector2[] FindPath(IEntity entity, Vector2 startTile, Vector2 endTile, LevelData level);
}
