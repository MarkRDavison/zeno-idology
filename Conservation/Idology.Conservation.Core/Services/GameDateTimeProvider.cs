namespace Idology.Conservation.Core.Services;

internal sealed class GameDateTimeProvider : IGameDateTimeProvider
{
    private DateTime _now = DateTime.MinValue;

    public void Increment(TimeSpan offset)
    {
        _now += offset;
    }

    public void Set(DateTime now)
    {
        _now = now;
    }

    public void SetPauseState(bool paused)
    {
        IsPaused = paused;
    }
    public void SetTimeSpeed(int speed)
    {
        TimeSpeed = (float)speed;
    }

    public DateOnly Date => DateOnly.FromDateTime(Now);

    public TimeOnly Time => TimeOnly.FromDateTime(Now);

    public DateTime Now => _now;

    public float TimeSpeed { get; private set; } = 1.0f;
    public bool IsPaused { get; private set; }
}
