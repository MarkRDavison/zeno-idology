namespace Idology.UserInterface.Components;

public class ButtonBase : UiComponentBase
{
    public int ButtonBorderSize = 4;
    public bool Pressed { get; protected set; }
    public bool Hovered { get; protected set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public event EventHandler? OnClick;

    protected void Invoke()
    {
        OnClick?.Invoke(this, EventArgs.Empty);
    }

    public override void Update(float delta)
    {
        var buttonBounds = Measure();
        var mouse = Raylib.GetMousePosition();

        if ((buttonBounds.X <= mouse.X && mouse.X <= buttonBounds.X + buttonBounds.Width) &&
            (buttonBounds.Y <= mouse.Y && mouse.Y <= buttonBounds.Y + buttonBounds.Height))
        {
            Hovered = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Pressed = true;
            }

            if (Raylib.IsMouseButtonReleased(MouseButton.Left) && Pressed)
            {
                Invoke();
                Pressed = false;
            }
        }
        else
        {
            Hovered = false;
            Pressed = false;
        }
    }

    public override Rectangle Measure()
    {
        return new Rectangle(Position, Size);
    }

    public override Rectangle Draw()
    {
        return PanelBackground.Draw(
            Position,
            Size,
            ButtonBorderSize,
            Pressed ? Color.LightGray : Color.Gray,
            Pressed ? Color.Gray : Color.LightGray);
    }
}
