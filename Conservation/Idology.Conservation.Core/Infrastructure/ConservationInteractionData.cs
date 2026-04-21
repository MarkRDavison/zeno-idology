namespace Idology.Conservation.Core.Infrastructure;

public enum ScreenState
{
    Default = 0,
    Region = 1,
    Kakapo = 2,
    Staff = 3
};

public sealed class DefaultScreenData
{
    public int? SelectedRegion { get; set; }
}

public sealed class ConservationInteractionData
{

    public ScreenState ScreenState { get; set; } = ScreenState.Default;
    public DefaultScreenData DefaultScreenData { get; } = new();

}
