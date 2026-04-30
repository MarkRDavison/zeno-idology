namespace Idology.Conservation.Core.Data;

// TODO: move away from 2d list?
// Lookup based on coords etc?
public sealed class RegionData
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public Vector2 RegionOffset { get; init; }
    public List<Tile> Tiles { get; } = [];
}
