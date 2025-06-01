namespace Idology.UserInterface.Components;

public sealed class ResourceGroup : UiComponentBase
{
    public const int BorderSize = 4;
    public IList<(int Amount, string Icon)> Resources { get; set; } = [];
    private static Vector2 _individualSize = new Vector2(128, 64 - BorderSize * 4);

    public Vector2 Position { get; set; }

    public override Rectangle Measure()
    {
        var individualSize = MeasureIndividual(new()).Size;

        var groupBounds = PanelBackground.Measure(
            Position,
            new Vector2(
                BorderSize + BorderSize + (individualSize.X + BorderSize) * Resources.Count + BorderSize,
                individualSize.Y + BorderSize * 4));

        return groupBounds;
    }

    public override Rectangle Draw()
    {
        var individualSize = MeasureIndividual(new()).Size;

        var groupBounds = PanelBackground.Draw(
            Position,
            new Vector2(
                BorderSize + BorderSize + (individualSize.X + BorderSize) * Resources.Count + BorderSize,
                individualSize.Y + BorderSize * 4),
            BorderSize);

        var position = new Vector2(Position.X + BorderSize * 2, Position.Y + BorderSize * 2);

        foreach (var (amount, icon) in Resources)
        {
            var bounds = DrawIndividual(position, amount, icon);

            position.X += bounds.Width + BorderSize;
        }

        return groupBounds;
    }

    private Rectangle MeasureIndividual(Vector2 position)
    {
        return new Rectangle(position, _individualSize);
    }

    private Rectangle DrawIndividual(Vector2 position, int amount, string icon)
    {
        var bounds = PanelBackground.Draw(position, _individualSize, BorderSize);

        const int FontSize = 32;

        var text = amount == 0 ? "-" : $"{amount}"; // TODO: If > 1000 abbreviate to 1.2k etc 1k, 1m etc etc

        var textSize = Raylib.MeasureText(text, FontSize);

        Raylib.DrawText(
            text,
            (int)(position.X + BorderSize * 3),
            (int)(position.Y + bounds.Height / 2 - FontSize / 2),
            FontSize,
            Color.Black);

        var texture = TextureManager.GetTexture(icon);

        Raylib.DrawTexturePro(
            texture,
            new Rectangle(0, 0, new Vector2(texture.Width, texture.Height)),
            new Rectangle(
                    position.X + bounds.Width - texture.Width - BorderSize * 2,
                    position.Y + bounds.Height / 2 - texture.Height / 2, 32, 32),
            new Vector2(),
            0.0f,
            Color.White);

        return bounds;
    }
}
