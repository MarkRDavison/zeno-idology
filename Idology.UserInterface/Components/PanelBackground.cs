namespace Idology.UserInterface.Components;

public static class PanelBackground
{
    public static Rectangle Measure(Vector2 position, Vector2 size)
    {
        return new Rectangle(position, size);
    }

    public static Rectangle Draw(
        Vector2 position,
        Vector2 size,
        int borderSize)
    {
        return Draw(position, size, borderSize, Color.Gray, Color.LightGray);
    }
    public static Rectangle Draw(
        Vector2 position,
        Vector2 size,
        int borderSize,
        Color borderColor,
        Color innerColor)
    {
        Raylib.DrawRectangle(
            (int)position.X,
            (int)position.Y,
            (int)size.X,
            (int)size.Y,
            borderColor);

        Raylib.DrawRectangle(
            (int)position.X + borderSize,
            (int)position.Y + borderSize,
            (int)(size.X - borderSize * 2),
            (int)(size.Y - borderSize * 2),
            innerColor);

        return new Rectangle(position, size);
    }
}
