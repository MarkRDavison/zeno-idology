namespace Idology.Conservation.Core.State;

internal sealed class ConservationStateService : IConservationStateService
{
    public ConservationStateService()
    {
        State = ConservationStateInitializationMutations.CreateDefaultData();
    }

    public ConservationGameData State { get; private set; }

    public void SetState(ConservationGameData newState)
    {
        State = newState;
        // TODO: Trigger change etc
    }
    public void SetState(Func<ConservationGameData, ConservationGameData> stateSelector)
    {
        SetState(stateSelector(State));
    }
}
