using Idology.Engine.Resources;
using Raylib_cs;
using System.Numerics;

namespace Idology.UserInterface;

public class TheInterface
{
    private readonly ITextureManager _textureManager;

    public TheInterface(
        ITextureManager textureManager)
    {
        _textureManager = textureManager;
    }

    const int BorderSize = 4;
    const int MarginSize = 16;
    public void Draw()
    {
        var position = new Vector2(256, 0);
        var size = new Vector2(192, 64);
        const string Label = "Click me";
        const int FontSize = 32;

        DrawLabelButton(position, size, Label, FontSize);

        DrawResourceGroupDisplaysWithIcons(
            position + new Vector2(256, 0),
            [
                (35, "test"),
                (53, "icon"),
                (35, "test"),
                (53, "icon"),
                (35, "test"),
                (53, "icon"),
            ]);
    }

    public void DrawLabelButton(Vector2 position, Vector2 size, string label, int fontSize)
    {
        DrawPanelBackground(position, size);

        var textSize = Raylib.MeasureText(label, fontSize);

        var decrease = 1;
        var newLabel = label;
        const string Ellipses = "...";
        while (textSize > size.X - BorderSize * 2)
        {
            newLabel = label.Substring(0, label.Length - Ellipses.Length - decrease) + Ellipses;
            decrease++;
            textSize = Raylib.MeasureText(newLabel, fontSize);
        }

        Raylib.DrawText(
            newLabel,
            (int)(position.X + size.X / 2 - textSize / 2),
            (int)(position.Y + size.Y / 2 - fontSize / 2),
            fontSize,
            Color.Black);
    }

    public void DrawResourceGroupDisplaysWithIcons(
        Vector2 position,
        IList<(int, string)> resourceInfo)
    {
        var count = resourceInfo.Count;
        var individualSize = new Vector2(128, 64);
        var individualSizeLessMargin = new Vector2(individualSize.X - BorderSize * 2, individualSize.Y - BorderSize * 2);

        var groupSize = new Vector2(
            individualSizeLessMargin.X * count + (BorderSize * (3 - count)),
            individualSize.Y);

        DrawPanelBackground(position, groupSize);

        var index = 0;
        var difference = individualSizeLessMargin.X - BorderSize;
        foreach (var r in resourceInfo)
        {
            var offset = new Vector2(difference * index, 0);
            DrawResourceDisplayWithIcon(
                position + offset,
                individualSize,
                r.Item1,
                r.Item2);

            index++;
        }
    }

    public void DrawResourceDisplayWithIcon(Vector2 position, Vector2 size, int amount, string icon)
    {
        var innerSize = new Vector2(size.X - MarginSize, size.Y - MarginSize);
        DrawPanelBackground(position + new Vector2(MarginSize / 2, MarginSize / 2), innerSize);

        const int FontSize = 32;

        Raylib.DrawText(
            $"{amount}", // TODO: If > 1000 abbreviate to 1.2k etc 1k, 1m etc etc
            (int)(position.X + MarginSize / 2 + BorderSize * 2),
            (int)(position.Y + BorderSize * 2 + MarginSize / 2),
            FontSize,
            Color.Black);

        var texture = _textureManager.GetTexture(icon);

        Raylib.DrawTexturePro(
            texture,
            new Rectangle(0, 0, new Vector2(texture.Width, texture.Height)),
            new Rectangle(position.X + size.X - texture.Width - MarginSize, position.Y + MarginSize, 32, 32),
            new Vector2(),
            0.0f,
            Color.White);
    }

    public void DrawPanelBackground(Vector2 position, Vector2 size)
    {
        Raylib.DrawRectangle(
            (int)position.X,
            (int)position.Y,
            (int)size.X,
            (int)size.Y,
            Color.Gray);

        Raylib.DrawRectangle(
            (int)position.X + BorderSize,
            (int)position.Y + BorderSize,
            (int)(size.X - BorderSize * 2),
            (int)(size.Y - BorderSize * 2),
            Color.LightGray);
    }
}
