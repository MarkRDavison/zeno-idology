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
    Region = 2
}

public sealed class DefaultScreenData
{
    public int? SelectedRegion { get; set; }
}

public sealed class ConservationInteractionData
{
    public InfoState InfoState { get; set; } = InfoState.Hidden;
    public ScreenState ScreenState { get; set; } = ScreenState.Default;
    public DefaultScreenData DefaultScreenData { get; } = new();
}
