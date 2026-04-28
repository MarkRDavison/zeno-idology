namespace Idology.Conservation.Core.Models;

public sealed record KakapoModel(
    int Id,
    string Name,
    Gender Gender,
    int? MotherId,
    int? FatherId,
    OriginInfo Origin,
    DateOnly? Death,
    int? RegionId);