namespace Idology.Space.Core.Commands.FindPath;

public sealed class FindPathCommand : ISpaceCommand
{
    public required IEntity Entity { get; set; }
    public required Vector2 StartTile { get; set; }
    public required Vector2 EndTile { get; set; }
    public required LevelData LevelData { get; set; }
}
