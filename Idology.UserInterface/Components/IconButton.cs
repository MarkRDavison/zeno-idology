namespace Idology.UserInterface.Components;

public class IconButton : ButtonBase
{
    public string Icon { get; set; } = string.Empty;

    public override Rectangle Draw()
    {
        var bounds = base.Draw();

        if (!string.IsNullOrEmpty(Icon))
        {
            var texture = TextureManager.GetTexture(Icon);

            Raylib.DrawTexturePro(
                texture,
                new Rectangle(0, 0, new Vector2(texture.Width, texture.Height)),
                new Rectangle(
                        Position.X + bounds.Width - texture.Width - ButtonBorderSize * 2,
                        Position.Y + bounds.Height / 2 - texture.Height / 2,
                        texture.Width, texture.Height),
                new Vector2(),
                0.0f,
                Hovered ? Color.White : Color.Black);
        }

        return bounds;
    }
}
