namespace Idology.Space.Core.Data;

public interface IPositionedEntity : IEntity
{
    public Vector2 Position { get; set; }
    public List<Vector2> Path { get; set; }
}
