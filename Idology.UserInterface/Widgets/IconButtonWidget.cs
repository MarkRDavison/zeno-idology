namespace Idology.UserInterface.Widgets;

public sealed class IconButtonWidget : BaseWidget
{
    private bool _mouseDownWithin;
    private bool _mouseWithin;

    public override void Update(float delta)
    {
        if (LayoutBoundsContainMousePosition())
        {
            if (!_mouseDownWithin && InputManager.HandleActionIfInvoked(UserInterfaceConstants.PRIMARY_CLICK_START))
            {
                _mouseDownWithin = true;
            }
            else if (_mouseDownWithin && InputManager.HandleActionIfInvoked(UserInterfaceConstants.PRIMARY_CLICK_END))
            {
                OnClick?.Invoke(this, EventArgs.Empty);
                _mouseDownWithin = false;
            }

            _mouseWithin = true;
        }
        else
        {
            _mouseWithin = false;
            _mouseDownWithin = false;
        }
    }

    public override void Draw()
    {
        if (BorderThickness.GetValueOrDefault() > 0)
        {
            Raylib.DrawRectangleLinesEx(
            new Rectangle(
                (int)Layout.Rect.X,
                (int)Layout.Rect.Y,
                (int)Layout.Rect.Width,
                (int)Layout.Rect.Height),
            BorderThickness ?? 0,
            _mouseWithin ? Color.Magenta : Border);
        }

        Raylib.DrawRectangle(
            (int)Layout.Rect.X + (int)(BorderThickness ?? 0),
            (int)Layout.Rect.Y + (int)(BorderThickness ?? 0),
            (int)Layout.Rect.Width - (int)(BorderThickness ?? 0) * 2,
            (int)Layout.Rect.Height - (int)(BorderThickness ?? 0) * 2,
            Background);

        Raylib.DrawTexture(
            UserInterfaceRoot.TextureManager.GetTexture(IconTextureName),
            (int)Layout.Rect.X + (int)(BorderThickness ?? 0) + (int)Layout.RequestedPadding.Left,
            (int)Layout.Rect.Y + (int)(BorderThickness ?? 0) + (int)Layout.RequestedPadding.Right,
            _mouseWithin ? (_mouseDownWithin ? Color.Red : Color.Yellow) : Foreground);
    }

    public override void PostConstructInit()
    {
        base.PostConstructInit();
        RecalculateSize();
    }

    internal void RecalculateSize()
    {
        var texture = UserInterfaceRoot.TextureManager.GetTexture(IconTextureName);

        Layout.RequestedSize = new LayoutVector(
            texture.Width + (int)(BorderThickness ?? 0) + Layout.RequestedPadding.Left + Layout.RequestedPadding.Right,
            texture.Height + (int)(BorderThickness ?? 0) + Layout.RequestedPadding.Top + Layout.RequestedPadding.Bottom);
    }

    public string IconTextureName { get; set; } = string.Empty;
    public event EventHandler? OnClick;
}
