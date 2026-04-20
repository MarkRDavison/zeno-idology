namespace Idology.Conservation.Core.Data;

public sealed class RegionData
{
    public int Width { get; init; }
    public int Height { get; init; }
    public List<Tile> Tiles { get; } = [];
}
