namespace Idology.Outpost.Core.Services.People;

public interface IPersonSpawnService
{
    void HandleSunrise();
    void HandleSunset();

    Person SpawnHunterAtSunrise();
    Person SpawnLumberjackAtSunrise();
    Person SpawnGuardAtSunset(int index, TownRegion homeRegion);
}
