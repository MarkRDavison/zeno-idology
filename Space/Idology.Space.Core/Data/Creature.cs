namespace Idology.Space.Core.Data;

public class Creature : IEntity
{
    public Guid Id { get; set; }
    public Guid PrototypeId { get; set; }
    public Vector2? TargetTile { get; set; }
    public Vector2 Position { get; set; } = new();
    public Color Color { get; set; } = Color.Pink;
}
