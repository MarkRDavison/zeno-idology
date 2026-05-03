namespace Idology.Conservation.Core.Data;

[Flags]
public enum TileType
{
    Unset = 0,
    Water = 1,
    Land = 2,
    Bush = 3,
    Beach = 4,
    Cliff = 5
}

public sealed record Tile(TileType TileType)
{
    public Color Color
    {
        get
        {
            if (TileType == TileType.Water)
            {
                return Color.Blue;
            }
            else if (TileType == TileType.Land)
            {
                return Color.Green;
            }
            else if (TileType == TileType.Bush)
            {
                return Color.DarkGreen;
            }
            else if (TileType == TileType.Beach)
            {
                return Color.Yellow;
            }
            else if (TileType == TileType.Cliff)
            {
                return Color.Gray;
            }

            return Color.Black;
        }
    }
}
