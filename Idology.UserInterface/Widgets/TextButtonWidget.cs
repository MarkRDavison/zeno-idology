namespace Idology.UserInterface.Widgets;

// TODO: Content button instead?
public sealed class TextButtonWidget : BaseWidget
{
    private bool _mouseDownWithin;
    private bool _mouseWithin;

    public override void Update(float delta)
    {
        if (LayoutBoundsContainMousePosition())
        {
            if (!_mouseDownWithin && InputManager.HandleActionIfInvoked("LCLICK_DOWN"))
            {
                _mouseDownWithin = true;
            }
            else if (_mouseDownWithin && InputManager.HandleActionIfInvoked("LCLICK_UP"))
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
        const int TextSize = 64;
        const int BorderSize = 8;

        Raylib.DrawRectangleLinesEx(
            new Rectangle(
                (int)Layout.Rect.X,
                (int)Layout.Rect.Y,
                (int)Layout.Rect.Width,
                (int)Layout.Rect.Height),
            BorderSize,
            _mouseWithin ? Color.Magenta : Border);

        Raylib.DrawRectangle(
            (int)Layout.Rect.X + BorderSize,
            (int)Layout.Rect.Y + BorderSize,
            (int)Layout.Rect.Width - BorderSize * 2,
            (int)Layout.Rect.Height - BorderSize * 2,
            _mouseDownWithin ? Color.Orange : Background);

        var text = "Hello world";

        // TODO: PADDING...
        var maxTextSize = Layout.Rect.Width - BorderSize * 2;

        while (true)
        {
            var textBounds = Raylib.MeasureText(text, TextSize);

            if (textBounds <= maxTextSize)
            {
                var buttonCenterX = (int)Layout.Rect.X + (int)Layout.Rect.Width / 2;
                var buttonCenterY = (int)Layout.Rect.Y + (int)Layout.Rect.Height / 2;

                Raylib.DrawText(text, buttonCenterX - textBounds / 2, buttonCenterY - TextSize / 2, TextSize, Foreground);

                break;
            }

            if (text.Length <= 0)
            {
                break;
            }

            text = text[..^1];
            // TODO: CACHE TEXT THAT FITS...
        }
    }

    public event EventHandler? OnClick;
}
