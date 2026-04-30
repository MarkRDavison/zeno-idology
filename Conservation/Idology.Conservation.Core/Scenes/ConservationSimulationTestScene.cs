namespace Idology.Conservation.Core.Scenes;

public sealed class ConservationSimulationTestScenePayload : IScenePayload<ConservationSimulationTestScene>
{

}

internal enum TileTypeEnum
{
    Water,
    Forest,
    Sand
};

public sealed class ConservationSimulationTestScene : ConservationScene<ConservationSimulationTestScene>
{
    private const int Height = 24;
    private const int Width = 24;

    private const int TileSize = 64;

    private readonly List<KakapoSimulationData> _kakapo = [];
    private readonly Dictionary<Vector2, TileTypeEnum> _tiles = [];
    private readonly HashSet<Vector2> _validCells = [];

    private ConservationGameCamera _camera;

    private readonly IInputManager _inputManager;

    public ConservationSimulationTestScene(IInputManager inputManager)
    {
        _inputManager = inputManager;

        _camera = new ConservationGameCamera(_inputManager)
        {
            // TODO: Better way of applying defaults...
            Target = new Vector2(1280, 800),
            Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
            Zoom = 0.4f
        };
    }

    public override void Init(IScenePayload<ConservationSimulationTestScene>? payload)
    {
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                var tileType = TileTypeEnum.Forest;

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tileType = TileTypeEnum.Sand;
                }

                if (tileType is TileTypeEnum.Forest)
                {
                    _validCells.Add(new Vector2(x, y));
                }

                _tiles.Add(new Vector2(x, y), tileType);
            }
        }
        const int Other = 8;

        _kakapo.Add(new KakapoSimulationData(1, 1, new Vector2(Other, Other)));
        _kakapo.Add(new KakapoSimulationData(1, 1, new Vector2(Width - Other, Other)));
        _kakapo.Add(new KakapoSimulationData(1, 1, new Vector2(Other, Height - Other)));
        _kakapo.Add(new KakapoSimulationData(1, 1, new Vector2(Width - Other, Height - Other)));

        _kakapo.Add(new KakapoSimulationData(1, 1, new Vector2(12, 12)));
        _kakapo.Add(new KakapoSimulationData(1, 1, new Vector2(13, 12)));
        _kakapo.Add(new KakapoSimulationData(1, 1, new Vector2(12, 13)));
        _kakapo.Add(new KakapoSimulationData(1, 1, new Vector2(13, 13)));
    }

    public override void Update(float delta)
    {
        if (_inputManager.HandleActionIfInvoked(Constants.Action_PlayPause))
        {
            Console.WriteLine("ITERATE");

            IslandRelaxation.Relax(_kakapo, _validCells, 1, Random.Shared, Width, Height);
        }

        _inputManager.Update();
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();

        Raylib.BeginMode2D(_camera.Camera);

        Raylib.ClearBackground(Color.DarkBlue);

        foreach (var tile in _tiles)
        {
            var col = tile.Value switch
            {
                TileTypeEnum.Water => Color.SkyBlue,
                TileTypeEnum.Forest => Color.Green,
                TileTypeEnum.Sand => Color.Yellow,
                _ => Color.Black
            };

            Raylib.DrawRectangle(
                (int)tile.Key.X * TileSize,
                (int)tile.Key.Y * TileSize,
                TileSize,
                TileSize,
                col);
        }

        foreach (var k in _kakapo)
        {

            Raylib.DrawRectangle(
                (int)k.CurrentLocation.X * TileSize,
                (int)k.CurrentLocation.Y * TileSize,
                TileSize,
                TileSize,
                Color.DarkGreen);
        }

        Raylib.EndMode2D();

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }
}
