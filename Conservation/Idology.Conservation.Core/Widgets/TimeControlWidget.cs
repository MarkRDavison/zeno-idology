namespace Idology.Conservation.Core.Widgets;

// TODO: Should each part of this be a sub-widget?
internal sealed class TimeControlWidget : PanelWidget
{
    const int Padding = 4;

    private int? _mouseWithinIndex;
    private int? _mouseDownWithinIndex;

    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public TimeControlWidget(
        IGameDateTimeProvider gameDateTimeProvider)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
    }

    public override void Update(float delta)
    {
        if (LayoutBoundsContainMousePosition())
        {
            var mousePos = InputManager.GetMousePosition();

            var startX = (int)Layout.Rect.X;

            var isWithinHeight = Layout.Rect.Y <= mousePos.Y && mousePos.Y <= Layout.Rect.Y + Layout.Rect.Height;

            if (isWithinHeight && startX + Padding <= mousePos.X && mousePos.X <= startX + Padding + 32)
            {
                _mouseWithinIndex = 0;

                if (_mouseDownWithinIndex is null && InputManager.HandleActionIfInvoked(Constants.Action_Click_Start))
                {
                    _mouseDownWithinIndex = 0;
                }
                else if (_mouseDownWithinIndex is 0 && InputManager.HandleActionIfInvoked(Constants.Action_Click))
                {
                    OnTogglePause?.Invoke(this, EventArgs.Empty);
                    OnSelectSpeed?.Invoke(this, 1);
                    // TODO: SPLIT pause and play apart 
                    _mouseDownWithinIndex = null;
                }
            }
            else if (isWithinHeight && startX + Padding + 20 + 12 <= mousePos.X && mousePos.X <= startX + Padding + 32 + 20 + 20 + 12)
            {
                _mouseWithinIndex = 1;

                if (_mouseDownWithinIndex is null && InputManager.HandleActionIfInvoked(Constants.Action_Click_Start))
                {
                    _mouseDownWithinIndex = 1;
                }
                else if (_mouseDownWithinIndex is 1 && InputManager.HandleActionIfInvoked(Constants.Action_Click))
                {
                    OnSelectSpeed?.Invoke(this, 2);
                    _mouseDownWithinIndex = null;
                }
            }
            else if (isWithinHeight && startX + Padding + 20 + 20 + 12 + 12 <= mousePos.X && mousePos.X <= startX + Padding + 32 + 20 + 20 + 20 + 20 + 20 + 12 + 12)
            {
                _mouseWithinIndex = 2;

                if (_mouseDownWithinIndex is null && InputManager.HandleActionIfInvoked(Constants.Action_Click_Start))
                {
                    _mouseDownWithinIndex = 2;
                }
                else if (_mouseDownWithinIndex is 2 && InputManager.HandleActionIfInvoked(Constants.Action_Click))
                {
                    OnSelectSpeed?.Invoke(this, 4);
                    _mouseDownWithinIndex = null;
                }
            }
            else
            {
                _mouseWithinIndex = null;
                _mouseDownWithinIndex = null;
            }
        }
        else
        {
            _mouseWithinIndex = null;
            _mouseDownWithinIndex = null;
        }
    }

    private int DrawTriangle(int penX, int amount, Color col)
    {
        for (int i = 0; i < amount; ++i)
        {
            Raylib.DrawTriangle(
                new Vector2(penX, Padding + (int)Layout.Rect.Y + (int)Layout.Rect.Height - 2 * Padding),
                new Vector2(penX + 20, Padding + (int)Layout.Rect.Y + ((int)Layout.Rect.Height - 2 * Padding) / 2),
                new Vector2(penX, Padding + (int)Layout.Rect.Y),
                col);

            penX += 20;
        }

        return penX;
    }

    public override void Draw()
    {
        base.Draw();

        var col = Color.DarkGray;

        int penX = Padding + (int)(Layout.Rect.X + BorderThickness.GetValueOrDefault());

        if (_gameDateTimeProvider.IsPaused)
        {
            Raylib.DrawRectangle(penX, Padding + (int)Layout.Rect.Y, 8, (int)Layout.Rect.Height - 2 * Padding, _mouseWithinIndex == 0 ? Color.Yellow : Color.White);

            penX += 12;

            Raylib.DrawRectangle(penX, Padding + (int)Layout.Rect.Y, 8, (int)Layout.Rect.Height - 2 * Padding, _mouseWithinIndex == 0 ? Color.Yellow : Color.White);

            penX += 12;

            penX += 8;
        }
        else
        {
            penX = DrawTriangle(penX, 1, _mouseWithinIndex == 0 ? Color.Yellow : (_gameDateTimeProvider.TimeSpeed == 1.0f ? Color.White : col));

            penX += 12;
        }

        penX = DrawTriangle(penX, 2, _mouseWithinIndex == 1 ? Color.Yellow : (_gameDateTimeProvider.TimeSpeed == 2.0f ? Color.White : col));

        penX += 12;

        penX = DrawTriangle(penX, 3, _mouseWithinIndex == 2 ? Color.Yellow : (_gameDateTimeProvider.TimeSpeed == 4.0f ? Color.White : col));
    }

    // TODO: SINGLE ENUM? Pause/ Play/ 2x/ 3x
    public event EventHandler? OnTogglePause;
    public event EventHandler<int>? OnSelectSpeed;
}
