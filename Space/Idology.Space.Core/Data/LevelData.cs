namespace Idology.Space.Core.Data;

public sealed class LevelData
{
    public int Width { get; init; }
    public int Height { get; init; }

    public List<List<TileData>> Tiles { get; init; } = [];

    public List<Creature> Creatures { get; set; } = [];
    public IPositionedEntity? ActiveEntity { get; set; }
}
