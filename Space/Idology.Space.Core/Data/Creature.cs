namespace Idology.Space.Core.Data;

public class Creature : IPositionedEntity
{
    public Guid Id { get; set; }
    public Guid PrototypeId { get; set; }
    public Vector2? TargetTile { get; set; }
    public Vector2 Position { get; set; } = new();
    public Color Color { get; set; } = Color.Pink;
    public List<Vector2> Path { get; set; } = [];
}
