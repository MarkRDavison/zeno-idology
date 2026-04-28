namespace Idology.Conservation.Core.Data;

public enum Gender
{
    Unknown,
    Male,
    Female
}

public enum OriginDateType
{
    KnownBirth,
    EstimatedBirth,
    Discovered,
    Unknown
}

public record OriginInfo(
    DateOnly? Date,
    OriginDateType Type,
    string? Notes = null);

public record KakapoData(
    int Id,
    string Name,
    Gender Gender,
    int? MotherId,
    int? FatherId,
    OriginInfo Origin,
    DateOnly? Death);