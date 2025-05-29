namespace Idology.Engine.Resources.Data;

public record SpriteSheetItem(string Name, int X, int Y, int Width, int Height)
{
    public Rectangle Bounds => new Rectangle(X, Y, Width, Height);
}
