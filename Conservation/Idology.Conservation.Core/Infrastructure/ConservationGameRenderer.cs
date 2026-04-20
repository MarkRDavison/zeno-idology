namespace Idology.Conservation.Core.Infrastructure;

public sealed class ConservationGameRenderer
{
    private readonly ConservationGameData _gameData;

    public ConservationGameRenderer(ConservationGameData gameData)
    {
        _gameData = gameData;
    }

    public void Update(float delta)
    {

    }

    public void Draw(Camera2D camera)
    {
        const int TileSize = 64;

        Raylib.BeginMode2D(camera);

        foreach (var region in _gameData.Regions)
        {
            for (int y = 0; y < region.Height; ++y)
            {
                for (int x = 0; x < region.Width; ++x)
                {
                    var tile = region.Tiles[y * region.Height + x];

                    Raylib.DrawRectangle(
                        x * TileSize + (int)region.RegionOffset.X * TileSize,
                        y * TileSize + (int)region.RegionOffset.Y * TileSize,
                        TileSize,
                        TileSize,
                        tile.Color);
                }
            }
        }


        Raylib.EndMode2D();
    }
}
