namespace Idology.Core.Systems;

public sealed class LevelRenderSystem : WorldSystem
{
    private readonly ISpriteSheetManager _spriteSheetManager;

    public LevelRenderSystem(ISpriteSheetManager spriteSheetManager)
    {
        _spriteSheetManager = spriteSheetManager;
    }

    public override void Update(World world, float delta)
    {
        foreach (var e in world.GetWithAll<LevelChunkComponent>())
        {
            var l = e.Get<LevelChunkComponent>();
            for (int y = 0; y < GameConstants.ChunkSize; ++y)
            {
                for (int x = 0; x < GameConstants.ChunkSize; ++x)
                {
                    Raylib.DrawRectangle(
                        GameConstants.ChunkSize * GameConstants.TileSize * (int)l.Coordinates.X + x * GameConstants.TileSize,
                        GameConstants.ChunkSize * GameConstants.TileSize * (int)l.Coordinates.Y + y * GameConstants.TileSize,
                        GameConstants.TileSize,
                        GameConstants.TileSize,
                        ((x + y) % 2 == 0) ? Color.Green : Color.Blue);
                }
            }
        }
    }
}