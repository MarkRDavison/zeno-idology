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

        Layout = new LayoutItem
        {
            RequestedSize = new LayoutVector(178, 0),
            Contain = ContainFlags.Row,
            Behave = BehaveFlags.VFill
        };
    }

    public override void Update(float delta)
    {
        if (!LayoutBoundsContainMousePosition())
        {
            _mouseWithinIndex = null;
            _mouseDownWithinIndex = null;
            return;
        }

        var mousePos = InputManager.GetMousePosition();
        var startX = (int)Layout.Rect.X;
        var isWithinHeight = Layout.Rect.Y <= mousePos.Y && mousePos.Y <= Layout.Rect.Y + Layout.Rect.Height;

        var PauseStartX = startX + Padding;
        var PauseEndX = PauseStartX + 20;

        var PlayStartX = PauseEndX + 8;
        var PlayEndX = PlayStartX + 20;

        var Play2StartX = PlayEndX + 8;
        var Play2EndX = Play2StartX + 40;

        var Play3StartX = Play2EndX + 8;
        var Play3EndX = Play3StartX + 60;

        var regions = new[]
        {
            (index: 0, startX: PauseStartX, endX: PauseEndX, timeMode: TimeMode.Paused),
            (index: 1, startX: PlayStartX, endX: PlayEndX, timeMode: TimeMode.Play),
            (index: 2, startX: Play2StartX, endX: Play2EndX, timeMode: TimeMode.Play2),
            (index: 3, startX: Play3StartX, endX: Play3EndX, timeMode: TimeMode.Play3)
        };

        foreach (var region in regions)
        {
            if (isWithinHeight && region.startX <= mousePos.X && mousePos.X <= region.endX)
            {
                _mouseWithinIndex = region.index;

                if (_mouseDownWithinIndex == null && InputManager.HandleActionIfInvoked(Constants.Action_Click_Start))
                {
                    _mouseDownWithinIndex = region.index;
                }
                else if (_mouseDownWithinIndex == region.index && InputManager.HandleActionIfInvoked(Constants.Action_Click))
                {

                    OnTimeModeChanged?.Invoke(this, region.timeMode);
                    _mouseDownWithinIndex = null;
                }

                break;
            }
        }
    }

    private Color TimeSpeedLogic(TimeMode timeMode)
    {
        return _gameDateTimeProvider.TimeMode == timeMode ? Color.White : Color.DarkGray;
    }

    public override void Draw()
    {
        base.Draw();

        var penX = Padding + (int)(Layout.Rect.X + BorderThickness.GetValueOrDefault());

        Raylib.DrawRectangle(penX, Padding + (int)Layout.Rect.Y, 8, (int)Layout.Rect.Height - 2 * Padding, _mouseWithinIndex == 0 ? Color.Yellow : TimeSpeedLogic(TimeMode.Paused));

        penX += 12;

        Raylib.DrawRectangle(penX, Padding + (int)Layout.Rect.Y, 8, (int)Layout.Rect.Height - 2 * Padding, _mouseWithinIndex == 0 ? Color.Yellow : TimeSpeedLogic(TimeMode.Paused));

        penX += 16;

        penX = DrawTriangle(penX, 1, _mouseWithinIndex == 1 ? Color.Yellow : TimeSpeedLogic(TimeMode.Play));
        penX += 8;

        penX = DrawTriangle(penX, 2, _mouseWithinIndex == 2 ? Color.Yellow : TimeSpeedLogic(TimeMode.Play2));
        penX += 8;

        penX = DrawTriangle(penX, 3, _mouseWithinIndex == 3 ? Color.Yellow : TimeSpeedLogic(TimeMode.Play3));
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

    public event EventHandler<TimeMode>? OnTimeModeChanged;
}
