namespace Idology.Conservation.Core.Services;

internal sealed class GameDateTimeProvider : IGameDateTimeProvider
{
    private DateTime _now = DateTime.MinValue;
    private TimeMode _lastNonPausedTimeMode = TimeMode.Play;
    public void Increment(TimeSpan offset)
    {
        TimeIncremented?.Invoke(this, offset);
        _now += offset;
    }

    public void Set(DateTime now)
    {
        _now = now;
    }

    public void SetTimeMode(TimeMode timeMode)
    {
        // TODO: Do we want pause twice to return to last speed???
        if (timeMode != TimeMode.Paused &&
            TimeMode == timeMode)
        {
            return;
        }

        if (timeMode is not TimeMode.Paused)
        {
            _lastNonPausedTimeMode = timeMode;
            TimeMode = timeMode;
        }
        else if (TimeMode is TimeMode.Paused)
        {
            TimeMode = _lastNonPausedTimeMode;
        }
        else
        {
            TimeMode = timeMode;
        }
    }

    public DateOnly Date => DateOnly.FromDateTime(Now);

    public TimeOnly Time => TimeOnly.FromDateTime(Now);

    public DateTime Now => _now;

    public TimeMode TimeMode { get; private set; } = TimeMode.Play;
    public bool IsPaused => TimeMode is TimeMode.Paused;
    public float TimeModeSpeed
    {
        get
        {
            return TimeMode switch
            {
                TimeMode.Paused => 0.0f,
                TimeMode.Play => 1.0f,
                TimeMode.Play2 => 2.0f,
                TimeMode.Play3 => 4.0f,
                _ => 0.0f
            };
        }
    }

    public event EventHandler<TimeSpan> TimeIncremented = default!;
}
