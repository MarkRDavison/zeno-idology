namespace Idology.Conservation.Core.Infrastructure;

public enum ScreenState
{
    Default = 0,
    Region = 1,
    Kakapo = 2,
    Staff = 3,
    Research = 4,
    Technology = 5,
    Funding = 6
};

public enum InfoState
{
    Hidden = 0,
    RegionSummary = 1,
    Region = 2,
    KakapoSummary = 3,
}

public sealed record DefaultScreenData(
    int? SelectedRegion);

public sealed record RegionScreenData(
    int? RegionId,
    int? SelectedKakapoId);

public sealed record ConservationInteractionData(
    InfoState InfoState,
    ScreenState ScreenState,
    DefaultScreenData DefaultScreenData,
    RegionScreenData RegionScreenData);