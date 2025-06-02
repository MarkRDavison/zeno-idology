using Idology.Engine.Resources;
using Idology.UserInterface.Components;

namespace Idology.Outpost.Core.Infrastructure;

public sealed class GameRenderer
{
    private readonly GameData _gameData;
    private readonly IResourceService _resourceService;
    private readonly IList<UiComponentBase> _components = [];

    private bool _resourcesChanged;

    public GameRenderer(
        GameData gameData,
        IResourceService resourceService,
        ITextureManager textureManager)
    {
        _gameData = gameData;
        _resourceService = resourceService;
        _resourceService.OnResourcesChanged += _resourceService_OnResourcesChanged;

        //_components.Add(new IconButton
        //{
        //    TextureManager = textureManager,
        //    Size = new Vector2(64, 64),
        //    Position = new Vector2(400, 0),
        //    Icon = "arrow"
        //});
        //_components.Add(new LabelButton
        //{
        //    TextureManager = textureManager,
        //    Label = "Click me!",
        //    Size = new Vector2(192, 64),
        //    Position = new Vector2(192, 0)
        //});
        _components.Add(new ResourceGroup
        {
            TextureManager = textureManager,
            Position = new Vector2(512, 0),
            Resources = GetResources()
        });

        _resourcesChanged = true;
    }

    private void _resourceService_OnResourcesChanged(object? sender, EventArgs e)
    {
        _resourcesChanged = true;
    }

    private IList<(int, string)> GetResources() => [.. _resourceService.GetResources().Select(_ => (_.Value.Current, _.Key.ToLower() + "_icon"))];

    public void Update(float delta)
    {
        foreach (var c in _components)
        {
            c.Update(delta);
        }

        if (_resourcesChanged && _components.FirstOrDefault(_ => _ is ResourceGroup) is ResourceGroup rg)
        {
            rg.Resources = GetResources();

            var bounds = rg.Measure();

            rg.Position = new Vector2(Raylib.GetScreenWidth() / 2 - bounds.Width / 2, 0);

            _resourcesChanged = false;
        }
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

                if (region.WallHealth > 0)
                {
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
                z.Mode == ZombieMode.Wandering ? Color.Pink : (z.Mode == ZombieMode.Moving ? Color.Maroon : Color.Red));

            if (z.Position != z.TargetPosition && z.TargetPosition is not null)
            {
                Raylib.DrawLine(
                    (int)z.Position.X,
                    (int)z.Position.Y,
                    (int)z.TargetPosition.Value.X,
                    (int)z.TargetPosition.Value.Y,
                    Color.White);
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

        foreach (var c in _components)
        {
            c.Draw();
        }
    }
}
