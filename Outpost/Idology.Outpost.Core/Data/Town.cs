namespace Idology.Outpost.Core.Data;

public sealed class Town
{
    public List<TownRegion> Regions { get; } = [];
    public List<Person> People { get; } = [];
    public TimeOfDay TimeOfDay { get; set; } = TimeOfDay.Day;
}
