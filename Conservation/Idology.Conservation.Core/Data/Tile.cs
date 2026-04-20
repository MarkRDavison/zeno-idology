namespace Idology.Conservation.Core.Data;

[Flags]
public enum TileType
{
    Unset = 0,
    Water = 1,
    Land = 2,
    Bush = 3,
    Beach = 4
}

public sealed record Tile(TileType TileType);
