namespace Idology.Conservation.Core.Data;

public record OriginInfo(
    DateOnly? Date,
    OriginDateType Type,
    string? Notes = null);

public record KakapoSeedData(
    int Id,
    string Name,
    Gender Gender,
    int? MotherId,
    int? FatherId,
    OriginInfo Origin,
    DateOnly? Death);