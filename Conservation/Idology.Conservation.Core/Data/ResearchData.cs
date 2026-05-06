namespace Idology.Conservation.Core.Data;

public sealed record ResearchData(int Id, string Name, string Description, int Cost, int Researched, IReadOnlyList<int> Prerequisites);