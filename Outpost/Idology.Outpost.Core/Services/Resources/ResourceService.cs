namespace Idology.Outpost.Core.Services.Resources;

public sealed class ResourceService : IResourceService
{
    private readonly GameData _gameData;

    public event EventHandler OnResourcesChanged = default!;

    public ResourceService(GameData gameData)
    {
        _gameData = gameData;
    }

    public AmountRange GetResource(string resource)
    {
        if (!_gameData.Resources.ContainsKey(resource))
        {
            _gameData.Resources.Add(resource, new AmountRange());
        }

        return _gameData.Resources[resource].Clone();
    }

    public IDictionary<string, AmountRange> GetResources()
    {
        return _gameData.Resources.ToDictionary(
            _ => _.Key,
            _ => _.Value.Clone());
    }

    public void ReduceResources(IDictionary<string, int> resourceAmounts)
    {
        foreach (var (resource, amount) in resourceAmounts)
        {
            if (amount <= 0 || !_gameData.Resources.ContainsKey(resource))
            {
                continue;
            }

            var r = _gameData.Resources[resource];
            r.Current = Math.Max(r.Current - amount, r.Min);
            OnResourcesChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool TryReduceResources(IDictionary<string, int> resourceAmounts)
    {
        foreach (var (resource, amount) in resourceAmounts)
        {
            if (amount <= 0 || !_gameData.Resources.ContainsKey(resource))
            {
                continue;
            }

            var r = _gameData.Resources[resource];

            if (r.Current - amount < r.Min)
            {
                return false;
            }
        }

        ReduceResources(resourceAmounts);
        return true;
    }

    public bool CanReduceResources(IDictionary<string, int> resourceAmounts)
    {
        throw new NotImplementedException();
    }

    public void IncreaseResources(IDictionary<string, int> resourceAmounts)
    {
        foreach (var (resource, amount) in resourceAmounts)
        {
            if (amount <= 0 || !_gameData.Resources.ContainsKey(resource))
            {
                continue;
            }

            var r = _gameData.Resources[resource];
            r.Current = Math.Min(r.Current + amount, r.Max);
            OnResourcesChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool TryIncreaseResources(IDictionary<string, int> resourceAmounts)
    {
        foreach (var (resource, amount) in resourceAmounts)
        {
            if (amount <= 0 || !_gameData.Resources.ContainsKey(resource))
            {
                continue;
            }

            var r = _gameData.Resources[resource];

            if (r.Current + amount > r.Max)
            {
                return false;
            }
        }

        IncreaseResources(resourceAmounts);
        return true;
    }

    public bool CanIncreaseResources(IDictionary<string, int> resourceAmounts)
    {
        foreach (var (resource, amount) in resourceAmounts)
        {
            if (amount <= 0)
            {
                continue;
            }

            var r = GetResource(resource);

            if (r.Current + amount > r.Max)
            {
                return false;
            }
        }

        return true;
    }
}
