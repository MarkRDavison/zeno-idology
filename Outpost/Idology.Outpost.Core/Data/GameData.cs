namespace Idology.Outpost.Core.Data;

public sealed class GameData
{
    public Town Town { get; } = new();
    public Dictionary<string, AmountRange> Resources { get; } = [];
}
