namespace Idology.Outpost.Core.Infrastructure;

public sealed class Game
{
    private readonly GameData _gameData;
    private readonly IPersonMovementService _personMovementService;
    private readonly IPersonSpawnService _personSpawnService;
    private readonly IPersonWorkService _personWorkService;
    private readonly IZombieSpawnService _zombieSpawnService;
    private readonly IZombieWanderService _zombieWanderService;
    private readonly IZombieMovementService _zombieMovementService;

    public Game(
        GameData gameData,
        IPersonMovementService personMovementService,
        IPersonSpawnService personSpawnService,
        IPersonWorkService personWorkService,
        IZombieSpawnService zombieSpawnService,
        IZombieWanderService zombieWanderService,
        IZombieMovementService zombieMovementService)
    {
        _gameData = gameData;

        _gameData.Town.Regions.AddRange([
            new TownRegion { Coordinates = new Vector2(0, -1) },
            CreateHomeRegion(),
            new TownRegion { Coordinates = new Vector2(0, +1) },
            new TownRegion { Coordinates = new Vector2(1, -1) },
            new TownRegion { Coordinates = new Vector2(1, 0) },
            new TownRegion { Coordinates = new Vector2(1, +1) }]);

        _personMovementService = personMovementService;
        _personSpawnService = personSpawnService;
        _personWorkService = personWorkService;
        _zombieSpawnService = zombieSpawnService;
        _zombieWanderService = zombieWanderService;
        _zombieMovementService = zombieMovementService;
    }

    public void ApplyCommand(IGameCommand command)
    {
        switch (command.Name)
        {
            case "SUNRISE":
                HandleSunrise();
                break;
            case "SUNSET":
                HandleSunset();
                break;
            case "SPAWN_ZOMBIE":
                HandleSpawnZombie();
                break;
        }
    }

    private void HandleSpawnZombie()
    {
        _zombieSpawnService.SpawnTestZombie();
    }

    private void HandleSunrise()
    {
        _gameData.Town.TimeOfDay = TimeOfDay.Day;
        _personSpawnService.HandleSunrise();
        _personMovementService.HandleSunrise();
        _gameData.Town.Zombies.Clear();
    }

    private void HandleSunset()
    {
        _gameData.Town.TimeOfDay = TimeOfDay.Night;
        _personSpawnService.HandleSunset();
        _personMovementService.HandleSunset();
    }

    public void Update(float delta)
    {
        _personMovementService.Update(delta);
        _personWorkService.Update(delta);
        _zombieMovementService.Update(delta);
        _zombieWanderService.Wander(delta);
    }

    private static TownRegion CreateHomeRegion()
    {
        var totalHeight = GameConstants.RegionHeight * GameConstants.TileSize;
        var gap = totalHeight / GameConstants.GuardsOnWall;
        return new TownRegion
        {
            Coordinates = new Vector2(0, 0),
            Unlocked = true,
            GuardPositions = [..Enumerable
                .Range(0, GameConstants.GuardsOnWall)
                .Select(_ => new Vector2(GameConstants.PersonRadius, gap / 2.0f + _ * gap))],
            SpawnerLocations =
            [
                new Vector2(- GameConstants.TileSize * 12, +GameConstants.TileSize * 4 + GameConstants.TileSize * 4),
                new Vector2(- GameConstants.TileSize * 12, -GameConstants.TileSize * 4 + GameConstants.TileSize * 4)
            ]
        };
    }
}
