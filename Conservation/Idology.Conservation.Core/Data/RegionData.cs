namespace Idology.Conservation.Core.Data;

public sealed class RegionData
{
    public int Width { get; init; }
    public int Height { get; init; }
    public Vector2 RegionOffset { get; init; }
    public List<Tile> Tiles { get; } = [];
}
