namespace Idology.Outpost.Core.Services.Zombies;

public sealed class ZombieSpawnService : IZombieSpawnService
{
    private readonly GameData _gameData;
    private readonly IPrototypeService<ZombiePrototype, Zombie> _zombiePrototypeService;

    public ZombieSpawnService(
        GameData gameData,
        IPrototypeService<ZombiePrototype, Zombie> zombiePrototypeService)
    {
        _gameData = gameData;
        _zombiePrototypeService = zombiePrototypeService;
    }

    public void SpawnTestZombie()
    {
        if (_gameData.Town.TimeOfDay is not TimeOfDay.Night)
        {
            return;
        }

        var range = (int)GameConstants.TileSize * 4;

        foreach (var spawnerLocation in _gameData.Town.Regions.SelectMany(_ => _.SpawnerLocations))
        {
            var zombie = _zombiePrototypeService.CreateEntity(PrototypeConstants.Zombie);

            zombie.Position = spawnerLocation + new Vector2(
                Random.Shared.Next(-range / 2, range / 2),
                Random.Shared.Next(-range / 2, range / 2));

            _gameData.Town.Zombies.Add(zombie);
        }
    }
}
