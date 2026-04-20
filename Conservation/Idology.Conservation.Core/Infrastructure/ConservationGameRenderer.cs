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

        //for (int y = 0; y < _gameData.WorldHeight; ++y)
        //{
        //    for (int x = 0; x < _gameData.WorldWidth; ++x)
        //    {
        //        var tile = _gameData.Tiles[y][x];

        //        Raylib.DrawRectangle(
        //            x * TileSize,
        //            y * TileSize,
        //            TileSize,
        //            TileSize,
        //            tile.Color);
        //    }
        //}

        Raylib.EndMode2D();
    }
}
