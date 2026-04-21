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

    public DateOnly Date => DateOnly.FromDateTime(Now);

    public TimeOnly Time => TimeOnly.FromDateTime(Now);

    public DateTime Now => _now;
}
