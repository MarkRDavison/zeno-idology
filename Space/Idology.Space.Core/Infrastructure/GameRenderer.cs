namespace Idology.Space.Core.Infrastructure;

public sealed class GameRenderer
{
    private readonly GameData _gameData;

    private readonly HashSet<(int, int)> _validAtmosphere = [];

    public GameRenderer(GameData gameData)
    {
        _gameData = gameData;
    }

    private string Mode { get; set; } = string.Empty;

    public void Update(float delta)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F1))
        {
            Mode = "ATMOSPHERE";
            Console.WriteLine("Recalculate atmosphere");

        }
        else if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            Mode = string.Empty;
        }
    }

    public void Draw(Camera2D camera)
    {
        Raylib.BeginMode2D(camera);

        const int Margin = 1;

        if (_gameData.CurrentLevel is { } level)
        {
            for (int y = 0; y < level.Height; ++y)
            {
                for (int x = 0; x < level.Width; ++x)
                {
                    var tile = level.Tiles[y][x];

                    var color = Color.White;

                    if (!tile.IsEmpty)
                    {
                        color = Color.Red;
                    }
                    else
                    {
                        if (Mode == "ATMOSPHERE")
                        {
                            if (_validAtmosphere.Contains((x, y)))
                            {
                                color = Color.Green;
                            }
                            else
                            {
                                color = Color.Maroon;
                            }
                        }
                    }

                    Raylib.DrawRectangle(
                        x * SpaceConstants.TileSize + Margin,
                        y * SpaceConstants.TileSize + Margin,
                        SpaceConstants.TileSize - 2 * Margin,
                        SpaceConstants.TileSize - 2 * Margin,
                        color);
                }
            }

            foreach (var creature in level.Creatures)
            {
                var isActive = creature == level.ActiveEntity;

                if (isActive)
                {
                    Raylib.DrawCircle(
                        (int)(creature.Position.X * SpaceConstants.TileSize),
                        (int)(creature.Position.Y * SpaceConstants.TileSize),
                        SpaceConstants.TileSize / 2.5f,
                        Color.Green);
                }

                Raylib.DrawCircle(
                    (int)(creature.Position.X * SpaceConstants.TileSize),
                    (int)(creature.Position.Y * SpaceConstants.TileSize),
                    SpaceConstants.TileSize / 3f,
                    creature.Color);

                if (creature.Path is { Count: > 1 })
                {
                    for (int i = 1; i < creature.Path.Count; ++i)
                    {
                        Raylib.DrawLine(
                            (int)((creature.Path[i - 1].X) * (SpaceConstants.TileSize) + SpaceConstants.TileSize * 0.5f),
                            (int)((creature.Path[i - 1].Y) * (SpaceConstants.TileSize) + SpaceConstants.TileSize * 0.5f),
                            (int)((creature.Path[i + 0].X) * (SpaceConstants.TileSize) + SpaceConstants.TileSize * 0.5f),
                            (int)((creature.Path[i + 0].Y) * (SpaceConstants.TileSize) + SpaceConstants.TileSize * 0.5f),
                            Color.Magenta);
                    }
                }
            }
        }

        Raylib.EndMode2D();
    }
}
