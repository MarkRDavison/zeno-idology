namespace Idology.Outpost.Core.Data;

public sealed class TownRegion
{
    public Vector2 Coordinates { get; set; }
    public bool Unlocked { get; set; }
    public List<Vector2> GuardPositions { get; set; } = [];
    public List<Vector2> SpawnerLocations { get; set; } = [];
    public int WallHealth { get; set; } = 600;
}
