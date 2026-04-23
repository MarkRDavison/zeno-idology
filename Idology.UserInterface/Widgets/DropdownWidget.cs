namespace Idology.UserInterface.Widgets;

public sealed record DropdownItem(string Text, string Id, string? Icon = null);

public sealed class DropdownWidget : BaseWidget
{
    private bool _mouseDownWithin;
    private bool _mouseWithin;
    private bool _isDropDownOpen;
    private int? _hoveredDropdownIndex;

    public override void Update(float delta)
    {
        const int TextSize = 48;

        if (LayoutBoundsContainMousePosition())
        {
            if (!_mouseDownWithin && InputManager.HandleActionIfInvoked("LCLICK_DOWN"))
            {
                _mouseDownWithin = true;
            }
            else if (_mouseDownWithin && InputManager.HandleActionIfInvoked("LCLICK_UP"))
            {
                // Open dropdown
                _mouseDownWithin = false;
                _isDropDownOpen = true;
            }

            _mouseWithin = true;
        }
        else
        {
            _hoveredDropdownIndex = null;
            // TODO: register outside click handler???
            if (_isDropDownOpen)
            {
                var dropdownStart = new Vector2(Layout.Rect.X, (int)Layout.Rect.Y + (int)Layout.Rect.Height - (BorderThickness ?? 0));

                // TODO: Wont work when border size is small......
                var dropdownItemHeight = (TextSize + (BorderThickness ?? 0) * 4);

                var dropdownSize = new Vector2(Layout.Rect.Width, dropdownItemHeight * Items.Count);

                var mousePos = InputManager.GetMousePosition();

                if (dropdownStart.X <= mousePos.X && mousePos.X <= dropdownStart.X + dropdownSize.X &&
                    dropdownStart.Y <= mousePos.Y && mousePos.Y <= dropdownStart.Y + dropdownSize.Y)
                {
                    var yPosRelative = (mousePos.Y - dropdownStart.Y) / dropdownItemHeight;
                    _hoveredDropdownIndex = (int)Math.Round(yPosRelative, MidpointRounding.ToZero);
                }

                if (InputManager.IsActionInvoked("LCLICK_UP"))
                {
                    if (_hoveredDropdownIndex is { } idx)
                    {
                        InputManager.MarkActionAsHandled("LCLICK_UP");
                        OnItemSelected?.Invoke(this, Items.ElementAt(idx));
                    }

                    _isDropDownOpen = false;
                }
            }

            _mouseWithin = false;
            _mouseDownWithin = false;
        }
    }
    public override void Draw()
    {
        const int TextSize = 48;

        if (BorderThickness.GetValueOrDefault() > 0)
        {
            Raylib.DrawRectangleLinesEx(
            new Rectangle(
                (int)Layout.Rect.X,
                (int)Layout.Rect.Y,
                (int)Layout.Rect.Width,
                (int)Layout.Rect.Height),
            BorderThickness ?? 0,
            (!_isDropDownOpen && _mouseWithin) ? Color.Magenta : Border);
        }

        Raylib.DrawRectangle(
            (int)Layout.Rect.X + (int)(BorderThickness ?? 0),
            (int)Layout.Rect.Y + (int)(BorderThickness ?? 0),
            (int)Layout.Rect.Width - (int)(BorderThickness ?? 0) * 2,
            (int)Layout.Rect.Height - (int)(BorderThickness ?? 0) * 2,
            (!_isDropDownOpen && _mouseWithin) ? Color.Orange : Background);

        const int DropIconMargin = 12;

        int triSize = (int)Layout.Rect.Height - (int)(BorderThickness ?? 0) * 2 - DropIconMargin * 2;

        var triEndX = (int)Layout.Rect.X + (int)Layout.Rect.Width - (int)(BorderThickness ?? 0) - DropIconMargin;
        var triStartY = (int)Layout.Rect.Y + (int)(BorderThickness ?? 0) + DropIconMargin;

        Raylib.DrawTriangle(
            new Vector2(triEndX, triStartY),
            new Vector2(triEndX - triSize, triStartY),
            new Vector2(triEndX - triSize / 2, triStartY + triSize),
            (!_isDropDownOpen && _mouseWithin) ? Color.Magenta : Border);

        if (SelectedItemId is { } id && Items.FirstOrDefault(_ => _.Id == id) is { } item)
        {

            var text = item.Text;

            // TODO: PADDING...
            var maxTextSize = Layout.Rect.Width - (int)(BorderThickness ?? 0) * 2 - DropIconMargin * 2 - triSize;

            // Different approach if Desired size not set? Then update needs to set it?

            while (true)
            {
                var textBounds = Raylib.MeasureText(text, TextSize);

                if (textBounds <= maxTextSize)
                {
                    var textPosX = (int)Layout.Rect.X + (int)(Layout.Rect.Height - TextSize) - (int)(BorderThickness ?? 0);
                    var buttonCenterY = (int)Layout.Rect.Y + (int)Layout.Rect.Height / 2;

                    Raylib.DrawText(
                        text,
                        textPosX,
                        buttonCenterY - TextSize / 2,
                        TextSize,
                        Foreground);

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

        if (_isDropDownOpen)
        {
            var dropdownStart = new Vector2(Layout.Rect.X, (int)Layout.Rect.Y + (int)Layout.Rect.Height - (BorderThickness ?? 0));

            // TODO: Wont work when border size is small......
            var dropdownItemHeight = (TextSize + (BorderThickness ?? 0) * 4);

            var dropdownSize = new Vector2(Layout.Rect.Width, dropdownItemHeight * Items.Count);

            Raylib.DrawRectangleLinesEx(
                new Rectangle(
                    (int)dropdownStart.X,
                    (int)dropdownStart.Y,
                    (int)dropdownSize.X,
                    (int)dropdownSize.Y),
                BorderThickness ?? 0,
                Border);

            Raylib.DrawRectangle(
                (int)dropdownStart.X + (int)(BorderThickness ?? 0),
                (int)dropdownStart.Y + (int)(BorderThickness ?? 0),
                (int)dropdownSize.X - 2 * (int)(BorderThickness ?? 0),
                (int)dropdownSize.Y - 2 * (int)(BorderThickness ?? 0),
                Background);

            var penPos = dropdownStart;

            for (int idx = 0; idx < Items.Count; ++idx)
            {
                var ddi = Items.ElementAt(idx);

                {
                    var text = ddi.Text;

                    // TODO: PADDING...
                    var maxTextSize = Layout.Rect.Width - (int)(BorderThickness ?? 0) * 4;

                    // Different approach if Desired size not set? Then update needs to set it?

                    while (true)
                    {
                        var textBounds = Raylib.MeasureText(text, TextSize);

                        if (textBounds <= maxTextSize)
                        {
                            Raylib.DrawText(
                            text,
                            (int)(penPos.X + (BorderThickness ?? 0) * 4),
                            (int)(penPos.Y + (BorderThickness ?? 0) * 2),
                            TextSize,
                            _hoveredDropdownIndex == idx ? Color.Yellow : Color.White);

                            break;
                        }

                        if (text.Length <= 0)
                        {
                            break;
                        }

                        text = text[..^1];
                        // TODO: CACHE TEXT THAT FITS...
                    }

                    penPos.Y += dropdownItemHeight;
                }
            }
        }
    }

    public string? SelectedItemId { get; set; }

    public List<DropdownItem> Items { get; } = [];

    public event EventHandler<DropdownItem>? OnItemSelected;
}
