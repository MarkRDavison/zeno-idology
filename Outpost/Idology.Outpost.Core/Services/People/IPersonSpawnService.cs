namespace Idology.Outpost.Core.Services.People;

public interface IPersonSpawnService
{
    void HandleSunrise();
    void HandleSunset();

    Worker SpawnHunterAtSunrise();
    Worker SpawnLumberjackAtSunrise();
    Guard SpawnGuardAtSunset(int index, TownRegion homeRegion);
}
