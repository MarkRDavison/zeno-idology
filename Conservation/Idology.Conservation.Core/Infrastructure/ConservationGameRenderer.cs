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

        int regionIdx = 0;
        foreach (var region in _gameData.Regions)
        {
            for (int y = 0; y < region.Height; ++y)
            {
                for (int x = 0; x < region.Width; ++x)
                {
                    var tile = region.Tiles[y * region.Width + x];

                    Raylib.DrawRectangle(
                        x * TileSize + (int)region.RegionOffset.X * TileSize,
                        y * TileSize + (int)region.RegionOffset.Y * TileSize,
                        TileSize,
                        TileSize,
                        tile.Color);
                }
            }

            if (_gameData.InteractionData.DefaultScreenData.SelectedRegion == regionIdx)
            {
                Raylib.DrawRectangleLinesEx(
                    new Rectangle(
                        new Vector2(
                            (int)region.RegionOffset.X * TileSize,
                            (int)region.RegionOffset.Y * TileSize),
                        new Vector2(
                            TileSize * region.Width,
                            TileSize * region.Height)
                        ),
                    (float)(TileSize / 4.0f),
                    Color.White);
            }

            regionIdx++;
        }


        Raylib.EndMode2D();
    }
}
