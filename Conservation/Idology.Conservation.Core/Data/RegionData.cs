namespace Idology.Conservation.Core.Data;

public sealed class RegionData
{
    public required string Name { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public Vector2 RegionOffset { get; init; }
    public List<Tile> Tiles { get; } = [];
}
