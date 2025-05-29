namespace Idology.Outpost.Core.Infrastructure;

public sealed class Game
{
    private readonly GameData _gameData;
    private readonly IPersonMovementService _personMovementService;

    public Game(
        GameData gameData,
        IPersonMovementService personMovementService)
    {
        _gameData = gameData;
        _personMovementService = personMovementService;

        _gameData.Town.Regions.AddRange([
            new TownRegion { Coordinates = new Vector2(0, -1) },
            CreateHomeRegion(),
            new TownRegion { Coordinates = new Vector2(0, +1) },
            new TownRegion { Coordinates = new Vector2(1, -1) },
            new TownRegion { Coordinates = new Vector2(1, 0) },
            new TownRegion { Coordinates = new Vector2(1, +1) }]);
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
        }
    }

    private void HandleSunrise()
    {
        _gameData.Town.TimeOfDay = TimeOfDay.Day;
        _personMovementService.HandleSunrise();
    }

    private void HandleSunset()
    {
        _gameData.Town.TimeOfDay = TimeOfDay.Night;
        _personMovementService.HandleSunset();
    }

    public void Update(float delta)
    {
        _personMovementService.Update(delta);
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
                .Select(_ => new Vector2(GameConstants.PersonRadius, gap / 2.0f + _ * gap))]
        };
    }
}
