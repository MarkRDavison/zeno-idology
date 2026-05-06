namespace Idology.Conservation.Core.Widgets;

internal sealed class DateTimeWidget : PanelWidget
{
    private const int TimeSpanHours = 6;
    private const float RealSecondsPerGameTimeSpan = 4.0f;

    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly ITranslationService _translationService;

    private float _accumulatedRealTime = 0.0f;

    public DateTimeWidget(
        IGameDateTimeProvider gameDateTimeProvider,
        ITranslationService translationService)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _translationService = translationService;
    }

    public override void Update(float delta)
    {
        base.Update(delta);

        if (!_gameDateTimeProvider.IsPaused)
        {
            _accumulatedRealTime += delta;

            var threshold = RealSecondsPerGameTimeSpan / _gameDateTimeProvider.TimeModeSpeed;

            if (_accumulatedRealTime > threshold)
            {
                _accumulatedRealTime -= threshold;
                _gameDateTimeProvider.Increment(TimeSpan.FromHours(TimeSpanHours));
                // TODO: Better place for this
            }
        }
    }

    public override void Draw()
    {
        const int FontSize = 40;

        var content = _gameDateTimeProvider.Now.ToString(_translationService["TOP_BAR_DATE_FORMAT"]);

        var contentSize = Raylib.MeasureText(content, FontSize);

        var pos = new LayoutVector(Layout.Rect.X, Layout.Rect.Y);

        pos.Y += Layout.Rect.Height / 2.0f - FontSize / 2 + BorderThickness.GetValueOrDefault();
        pos.X += Layout.Rect.Width / 2.0f - contentSize / 2;

        base.Draw();

        Raylib.DrawText(content, (int)pos.X, (int)pos.Y, FontSize, Color.White);
    }
}
