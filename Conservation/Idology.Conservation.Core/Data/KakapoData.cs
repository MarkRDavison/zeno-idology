namespace Idology.Conservation.Core.Data;

// Need Birth, Death, Discovered etc
public record KakapoData(int Id, string Name, bool female, int? MotherId, int? FatherId, DateOnly? Birth, DateOnly? Death);