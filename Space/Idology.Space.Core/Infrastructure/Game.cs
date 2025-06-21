namespace Idology.Space.Core.Infrastructure;

public sealed class Game
{
    private readonly GameData _gameData;
    private readonly GameCamera _gameCamera;
    private readonly SpaceCommandHandler _commandHandler;
    private readonly IInputService _inputService;

    public Game(
        GameData gameData,
        GameCamera gameCamera,
        SpaceCommandHandler commandHandler,
        IInputService inputService)
    {
        _gameData = gameData;
        _gameCamera = gameCamera;
        _commandHandler = commandHandler;
        _inputService = inputService;
    }

    public void Update(float delta)
    {
        // TODO: Consolidate
        if (_inputService.IsMouseDoubleClicked(MouseButton.Left))
        {
            var world = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _gameCamera.Camera);
            var tileX = (int)(world.X / SpaceConstants.TileSize);
            var tileY = (int)(world.Y / SpaceConstants.TileSize);
            _commandHandler.HandleCommand(new SelectLocationCommand(tileX, tileY, true));
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Right) &&
            _gameData.CurrentLevel is { } level &&
            level.ActiveEntity is not null)
        {
            var world = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _gameCamera.Camera);
            var tileX = (int)(world.X / SpaceConstants.TileSize);
            var tileY = (int)(world.Y / SpaceConstants.TileSize);

            var path = _commandHandler.HandleCommandWithResult<FindPathCommand, Vector2[]>(new FindPathCommand
            {
                Entity = level.ActiveEntity,
                StartTile = new Vector2((int)Math.Floor(level.ActiveEntity.Position.X), (int)Math.Floor(level.ActiveEntity.Position.Y)),
                EndTile = new Vector2(tileX, tileY),
                LevelData = level
            });

            if (path is not null)
            {
                level.ActiveEntity.Path = [.. path];
            }
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Left))
        {
            _gameCamera.Offset = new(_gameCamera.Offset.X + SpaceConstants.TileSize, _gameCamera.Offset.Y);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Right))
        {
            _gameCamera.Offset = new(_gameCamera.Offset.X - SpaceConstants.TileSize, _gameCamera.Offset.Y);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Up))
        {
            _gameCamera.Offset = new(_gameCamera.Offset.X, _gameCamera.Offset.Y + SpaceConstants.TileSize);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Down))
        {
            _gameCamera.Offset = new(_gameCamera.Offset.X, _gameCamera.Offset.Y - SpaceConstants.TileSize);
        }

        _inputService.Update(delta);
    }
}
