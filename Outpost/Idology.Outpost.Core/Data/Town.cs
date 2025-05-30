using Idology.Outpost.Core.Data.Entities;

namespace Idology.Outpost.Core.Data;

public sealed class Town
{
    public List<TownRegion> Regions { get; } = [];
    public List<Person> People { get; } = [];
    public List<Zombie> Zombies { get; } = [];
    public TimeOfDay TimeOfDay { get; set; } = TimeOfDay.Day;
}
