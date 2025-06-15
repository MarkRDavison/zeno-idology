namespace Idology.Space.Core.Data;

public class Creature
{
    public Vector2? TargetTile { get; set; }
    public Vector2 Position { get; set; } = new();
    public Color Color { get; set; } = Color.Pink;
}
