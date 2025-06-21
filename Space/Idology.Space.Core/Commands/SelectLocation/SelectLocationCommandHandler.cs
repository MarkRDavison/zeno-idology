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

            var creatures = level.Creatures
                .Where(_ =>
                    command.X <= _.Position.X && _.Position.X < command.X + 1 &&
                    command.Y <= _.Position.Y && _.Position.Y < command.Y + 1)
                .ToList();

            var alreadySelectedInTile = command.Cycle && level.ActiveEntity is not null && creatures.Any(_ => _.Id == level.ActiveEntity.Id);

            IPositionedEntity? selected = null;
            IPositionedEntity? fallback = null;

            var takeNext = false;

            foreach (var c in creatures.OrderBy(_ => _.Id))
            {
                if (selected is null)
                {
                    fallback = c;
                    if (alreadySelectedInTile)
                    {
                        if (takeNext)
                        {
                            selected = c;
                        }
                        else if (c.Id == level.ActiveEntity?.Id)
                        {
                            takeNext = true;
                        }
                    }
                    else
                    {
                        selected = c;
                        break;
                    }
                }
            }

            if (selected is null && fallback is not null)
            {
                selected = fallback;
            }

            level.ActiveEntity = selected;
        }
    }
}
