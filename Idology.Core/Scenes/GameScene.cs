namespace Idology.Core.Scenes;

public sealed class GameScene : Scene
{
    private readonly World _world;
    private readonly IList<WorldSystem> _systems = [];
    private Camera2D _camera;
    private readonly IServiceProvider _serviceProvider;

    public GameScene(IServiceProvider serviceProvider)
    {
        _world = new World();
        _camera = new Camera2D();
        _serviceProvider = serviceProvider;
    }

    public override void Init()
    {
        _camera.Zoom = 1.0f;
        _camera.Offset = new Vector2(
            Raylib.GetScreenWidth() / 2,
            Raylib.GetScreenHeight() / 2)
            - new Vector2(8, 4) * GameConstants.TileSize;

        _serviceProvider.GetRequiredService<PrototypeCreation>().Init();

        _systems.Add(_serviceProvider.GetRequiredService<CareerAllocatorSystem>());
        _systems.Add(_serviceProvider.GetRequiredService<WorkplaceProcessSystem>());
        _systems.Add(_serviceProvider.GetRequiredService<LevelRenderSystem>());
        _systems.Add(_serviceProvider.GetRequiredService<RenderSystem>());
        _systems.Add(_serviceProvider.GetRequiredService<DebugUiSystem>());

        _world.Create(new LevelComponent());
        _world.Create(new LevelChunkComponent { Coordinates = new(0, 0) });
        _world.Create(new LevelChunkComponent { Coordinates = new(1, 0) });
        _world.Create(new LevelChunkComponent { Coordinates = new(0, 1) });
        _world.Create(new LevelChunkComponent { Coordinates = new(1, 1) });

        var buildingPrototypeService = _serviceProvider.GetRequiredService<IPrototypeService<BuildingPrototype, BuildingComponent>>();
        var workerPrototypeService = _serviceProvider.GetRequiredService<IPrototypeService<WorkerPrototype, WorkerComponent>>();

        _world.CreateBuilding(buildingPrototypeService, new Vector2(0, 0), "BasicTent");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(3, 1), "BasicTent");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(2, 4), "BasicTent");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(3, 4), "BasicTent");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(5, 5), "Bakery");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(6, 5), "Bakery");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(6, 2), "BasicField");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(7, 1), "BasicField");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(7, 3), "BasicField");
        _world.CreateBuilding(buildingPrototypeService, new Vector2(8, 2), "BasicField");

        _world.SummonWorkers(workerPrototypeService, 4);
    }

    public override void Update(float delta)
    {
        if (Raylib.GetMouseWheelMoveV() is { } v && v.Y != 0)
        {
            if (v.Y > 0)
            {
                _camera.Zoom *= 1.1f;
            }
            else
            {
                _camera.Zoom *= 1.0f / 1.1f;
            }
        }

        if (Raylib.IsKeyPressed(KeyboardKey.F1))
        {
            var workerPrototypeService = _serviceProvider.GetRequiredService<IPrototypeService<WorkerPrototype, WorkerComponent>>();

            _world.SummonWorkers(workerPrototypeService, 1);
        }
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        Raylib.DrawFPS(10, 10);

        Raylib.BeginMode2D(_camera);

        var delta = Raylib.GetFrameTime();

        for (int i = 0; i < _systems.Count; i++)
        {
            _systems[i].Update(_world, delta);
        }

        Raylib.EndMode2D();

        for (int i = 0; i < _systems.Count; i++)
        {
            _systems[i].UpdateNoCamera(_world, delta);
        }

        Raylib.EndDrawing();
    }
}
