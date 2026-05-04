namespace Idology.Conservation.Core.State;

public interface IConservationStateService
{
    ConservationGameData State { get; }
    void SetState(ConservationGameData newState);
    void SetState(Func<ConservationGameData, ConservationGameData> stateSelector);
}
