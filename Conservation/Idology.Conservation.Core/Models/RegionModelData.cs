namespace Idology.Conservation.Core.Models;

public sealed class RegionModelData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Vector2 Offset { get; set; }
}
