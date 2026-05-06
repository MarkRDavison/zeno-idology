namespace Idology.Conservation.Core.Services;

public enum TimeMode
{
    Paused,
    Play,
    Play2,
    Play3
}

public interface IGameDateTimeProvider
{
    DateOnly Date { get; }
    TimeOnly Time { get; }
    DateTime Now { get; }

    void Increment(TimeSpan offset);
    void Set(DateTime now);

    TimeMode TimeMode { get; }
    bool IsPaused { get; }

    void SetTimeMode(TimeMode timeMode);
    float TimeModeSpeed { get; }

    event EventHandler<TimeSpan> TimeIncremented;
}
