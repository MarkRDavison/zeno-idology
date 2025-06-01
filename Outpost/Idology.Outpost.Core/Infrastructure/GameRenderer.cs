using Idology.UserInterface;

namespace Idology.Outpost.Core.Infrastructure;

public sealed class GameRenderer
{
    private readonly GameData _gameData;
    private readonly IResourceService _resourceService;
    private readonly TheInterface _shitInterface;

    public GameRenderer(
        GameData gameData,
        IResourceService resourceService,
        TheInterface shitInterface)
    {
        _gameData = gameData;
        _resourceService = resourceService;
        _shitInterface = shitInterface;
    }

    public void Update(float _delta)
    {

    }

    public void Draw(Camera2D camera)
    {
        Raylib.BeginMode2D(camera);

        foreach (var region in _gameData.Town.Regions)
        {
            Raylib.DrawRectangle(
                (int)(region.Coordinates.X * GameConstants.RegionWidth * GameConstants.TileSize),
                (int)(region.Coordinates.Y * GameConstants.RegionHeight * GameConstants.TileSize),
                (int)(GameConstants.RegionWidth * GameConstants.TileSize),
                (int)(GameConstants.RegionHeight * GameConstants.TileSize),
                region.Unlocked ? Color.Green : Color.Red);

            if (region.Unlocked && region.Coordinates == new Vector2())
            {
                foreach (var guardLocation in region.GuardPositions)
                {
                    Raylib.DrawCircle(
                        (int)guardLocation.X,
                        (int)guardLocation.Y,
                        GameConstants.PersonRadius,
                        Color.White);
                }

                // Home region
                if (_gameData.Town.TimeOfDay == TimeOfDay.Day)
                {
                    Raylib.DrawRectangle(
                        -GameConstants.WallWidth,
                        0,
                        GameConstants.WallWidth,
                        (int)((GameConstants.RegionHeight * GameConstants.TileSize) - GameConstants.GateHeight) / 2,
                        Color.Brown);

                    Raylib.DrawRectangle(
                        -GameConstants.WallWidth,
                        (int)((GameConstants.RegionHeight * GameConstants.TileSize) - GameConstants.GateHeight) / 2 + GameConstants.GateHeight,
                        GameConstants.WallWidth,
                        (int)((GameConstants.RegionHeight * GameConstants.TileSize) - GameConstants.GateHeight) / 2,
                        Color.Brown);
                }
                else
                {
                    Raylib.DrawRectangle(
                        -GameConstants.WallWidth,
                        0,
                        GameConstants.WallWidth,
                        (int)(GameConstants.RegionHeight * GameConstants.TileSize),
                        Color.Brown);
                }

                Raylib.DrawRectangle(
                    (int)((region.Coordinates.X + 3) * GameConstants.TileSize),
                    (int)((region.Coordinates.Y + 3) * GameConstants.TileSize),
                    (int)(2 * GameConstants.TileSize),
                    (int)(2 * GameConstants.TileSize),
                    Color.Blue);
            }

            foreach (var spawner in region.SpawnerLocations)
            {
                Raylib.DrawCircle(
                    (int)spawner.X,
                    (int)spawner.Y,
                    GameConstants.PersonRadius * 2,
                    Color.Magenta);
            }
        }

        foreach (var p in _gameData.Town.People)
        {
            Raylib.DrawCircle(
                (int)p.Position.X,
                (int)p.Position.Y,
                GameConstants.PersonRadius,
                Color.Yellow);

            if (p.Position != p.TargetPosition)
            {
                Raylib.DrawLine((int)p.Position.X, (int)p.Position.Y, (int)p.TargetPosition.X, (int)p.TargetPosition.Y, Color.White);
            }
        }

        foreach (var z in _gameData.Town.Zombies)
        {
            Raylib.DrawCircle(
                (int)z.Position.X,
                (int)z.Position.Y,
                GameConstants.PersonRadius * 1.5f,
                z.Mode == ZombieMode.Wandering ? Color.Pink : Color.Maroon);

            if (z.Position != z.TargetPosition && z.TargetPosition is not null)
            {
                Raylib.DrawLine((int)z.Position.X, (int)z.Position.Y, (int)z.TargetPosition.Value.X, (int)z.TargetPosition.Value.Y, Color.White);
            }
        }

        // TODO: Temp -> basic zombie attraction threshold.
        Raylib.DrawLine(
            (int)(-GameConstants.TileSize * 7),
            (int)(-GameConstants.TileSize * 20),
            (int)(-GameConstants.TileSize * 7),
            (int)(+GameConstants.TileSize * 20),
            Color.Magenta);

        Raylib.DrawCircle(
            (int)GameConstants.HuntLocation.X,
            (int)GameConstants.HuntLocation.Y,
            GameConstants.PersonRadius * 5.0f,
            Color.DarkPurple);

        Raylib.DrawCircle(
            (int)GameConstants.ForestLocation.X,
            (int)GameConstants.ForestLocation.Y,
            GameConstants.PersonRadius * 5.0f,
            Color.DarkGreen);

        Raylib.EndMode2D();

        // UI

        var i = 0;
        foreach (var (r, rng) in _resourceService.GetResources())
        {
            Raylib.DrawText($"{r} x{rng.Current}", 10, 32 * i + 32, 24, Color.Black);
            i++;
        }

        _shitInterface.Draw();
    }
}
