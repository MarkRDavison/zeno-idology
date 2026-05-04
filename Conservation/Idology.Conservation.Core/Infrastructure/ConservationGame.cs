namespace Idology.Conservation.Core.Infrastructure;

public sealed class ConservationGame : IDisposable
{
    private bool _disposedValue;
    private readonly IConservationStateService _gameState;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public ConservationGame(
        IConservationStateService gameState,
        IGameDateTimeProvider gameDateTimeProvider)
    {
        _gameState = gameState;
        _gameDateTimeProvider = gameDateTimeProvider;

        _gameDateTimeProvider.TimeIncremented += OnTimeIncremented;
    }

    private void OnTimeIncremented(object? sender, TimeSpan e)
    {
        foreach (var rs in _gameState.State.RegionSimulations)
        {
            rs.Simulate(e);
        }
    }

    public void Update(float delta)
    {

    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _gameDateTimeProvider.TimeIncremented -= OnTimeIncremented;
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
