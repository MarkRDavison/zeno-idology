namespace Idology.Conservation.Core.Infrastructure;

public enum MainScreenState
{
    Default = 0,
    Region = 1
}

public enum ScreenPanelState
{
    None = 0,
    Kakapo = 1,
    Staff = 2,
    Research = 3,
    Technology = 4,
    Funding = 5
};

public enum InfoState
{
    RegionSummary = 0,
    Region = 1,
    KakapoSummary = 2,
}

public sealed record DefaultScreenData(
    int? SelectedRegion);

public sealed record RegionScreenData(
    int? RegionId,
    int? SelectedKakapoId);

public sealed record ConservationInteractionData(
    List<InfoState> InfoState,
    MainScreenState MainScreenState,
    ScreenPanelState PanelState,
    DefaultScreenData DefaultScreenData,
    RegionScreenData RegionScreenData);