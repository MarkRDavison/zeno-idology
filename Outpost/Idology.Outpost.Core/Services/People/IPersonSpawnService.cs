namespace Idology.Outpost.Core.Services.People;

public interface IPersonSpawnService
{
    void HandleSunrise();
    void HandleSunset();

    Worker SpawnHunterAtSunrise();
    Worker SpawnLumberjackAtSunrise();
    Worker SpawnGuardAtSunset(int index, TownRegion homeRegion);
}
