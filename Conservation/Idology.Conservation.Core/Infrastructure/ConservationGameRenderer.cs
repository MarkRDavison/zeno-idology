using Idology.Conservation.Core.Services;

namespace Idology.Conservation.Core.Infrastructure;

public sealed class ConservationGameRenderer
{
    private readonly ConservationGameData _gameData;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public ConservationGameRenderer(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider)
    {
        _gameData = gameData;
        _gameDateTimeProvider = gameDateTimeProvider;
    }

    public void Update(float delta)
    {

    }

    public void Draw(Camera2D camera)
    {
        const int TileSize = 64;


        if (_gameData.InteractionData.ScreenState is ScreenState.Default)
        {
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
        else if (_gameData.InteractionData.ScreenState is ScreenState.Region)
        {
            if (_gameData.ActiveRegion is { } activeRegion)
            {
                Raylib.BeginMode2D(camera);
                for (int y = 0; y < activeRegion.Height; ++y)
                {
                    for (int x = 0; x < activeRegion.Width; ++x)
                    {
                        var tile = activeRegion.Tiles[y * activeRegion.Width + x];

                        Raylib.DrawRectangle(
                            x * TileSize,
                            y * TileSize,
                            TileSize,
                            TileSize,
                            tile.Color);
                    }
                }
                Raylib.EndMode2D();
            }
        }
        else if (_gameData.InteractionData.ScreenState is ScreenState.Kakapo)
        {
            Raylib.DrawText("Kakapo details", 32, 32, 48, Color.White);

            const int Padding = 4;

            var yPos = 32 + 48 + Padding;

            foreach (var kd in _gameData.KakapoData)
            {
                var birdSummaryHeight = 0;

                var summaryLine = kd.Name;
                if (kd.Birth is not null)
                {
                    summaryLine += $"\t({_gameDateTimeProvider.Date.Year - kd.Birth.Value.Year})";
                }
                Raylib.DrawText(summaryLine, 32 + Padding, yPos, 32, Color.Black);

                birdSummaryHeight += 32;

                yPos += birdSummaryHeight + Padding;
            }
        }
        else if (_gameData.InteractionData.ScreenState is ScreenState.Staff)
        {
            Raylib.DrawText("Staff details", 32, 32, 48, Color.White);

            const int Padding = 4;

            var yPos = 32 + 48 + Padding;

            foreach (var kd in _gameData.StaffData)
            {
                var summaryHeight = 0;

                Raylib.DrawText(kd.Name, 32 + Padding, yPos, 32, Color.Black);

                summaryHeight += 32;

                yPos += summaryHeight + Padding;
            }
        }
    }
}
