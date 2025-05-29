namespace Idology.Outpost.Core;

public static class GameConstants
{
    public const float PersonRadius = 8.0f;
    public const float TileSize = 64.0f;
    public const int RegionWidth = 12;
    public const int RegionHeight = 8;
    public const int WallWidth = 16;
    public const int GateHeight = 128;
    public const int GuardsOnWall = 5;
    public static Vector2 HuntLocation => new(
        TileSize * -14,
        TileSize * +9);

    public static Vector2 MusterPoint => new(
        (int)(3 * GameConstants.TileSize),
        (int)(4 * GameConstants.TileSize));
}

public static class ResourceConstants
{
}