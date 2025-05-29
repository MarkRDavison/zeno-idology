namespace Idology.Engine.Ecs.Components;

public enum SpriteOffset
{
    None,
    Column
}
public record SpriteGraphicItem(string sheet, string name, SpriteOffset offset = SpriteOffset.None);

public class SpriteGraphicComponent
{
    public List<SpriteGraphicItem> Parts { get; } = [];
    public Color Color { get; set; } = Color.White;
}
