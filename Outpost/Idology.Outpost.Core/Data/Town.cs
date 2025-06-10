namespace Idology.Outpost.Core.Data;

public sealed class Town
{
    public List<TownRegion> Regions { get; } = [];
    public List<Person> People { get; } = [];
    public List<Zombie> Zombies { get; } = [];
    public IEnumerable<Worker> Workers => People.OfType<Worker>();
    public IEnumerable<Guard> Guards => People.OfType<Guard>();
    public TimeOfDay TimeOfDay { get; set; } = TimeOfDay.Day;
}
