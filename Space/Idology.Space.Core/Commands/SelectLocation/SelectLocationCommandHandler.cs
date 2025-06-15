namespace Idology.Space.Core.Commands.SelectLocation;

public sealed class SelectLocationCommandHandler : ISpaceCommandHandler<SelectLocationCommand>
{
    private readonly GameData _gameData;
    private readonly ILogger<SelectLocationCommandHandler> _logger;

    public SelectLocationCommandHandler(GameData gameData, ILogger<SelectLocationCommandHandler> logger)
    {
        _gameData = gameData;
        _logger = logger;
    }

    public void HandleCommand(SelectLocationCommand command)
    {
        if (_gameData.CurrentLevel is { } level)
        {
            if (command.X < 0 || command.X >= level.Width ||
                command.Y < 0 || command.Y >= level.Height)
            {
                _logger.LogWarning("Cannot select location outside of active level bounds");
                return;
            }

            var tile = level.Tiles[command.Y][command.X];

            _logger.LogInformation("Tile: {Tile}", JsonSerializer.Serialize(tile, SpaceConstants.DefaultJsonSerializerOptions));

            var creatures = level.Creatures.Where(_ =>
                command.X <= _.Position.X && _.Position.X < command.X + 1 &&
                command.Y <= _.Position.Y && _.Position.Y < command.Y + 1).ToList();

            foreach (var c in creatures)
            {

                _logger.LogInformation("Creature: {Creature}", JsonSerializer.Serialize(c, SpaceConstants.DefaultJsonSerializerOptions));
            }
        }
    }
}
