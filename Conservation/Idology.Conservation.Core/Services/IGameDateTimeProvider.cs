namespace Idology.Conservation.Core.Services;

public interface IGameDateTimeProvider
{
    DateOnly Date { get; }
    TimeOnly Time { get; }
    DateTime Now { get; }

    void Increment(TimeSpan offset);
    void Set(DateTime now);
}
