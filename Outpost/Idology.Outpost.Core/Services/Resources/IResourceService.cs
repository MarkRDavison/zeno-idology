namespace Idology.Outpost.Core.Services.Resources;

public interface IResourceService
{
    AmountRange GetResource(string resource);
    public IDictionary<string, AmountRange> GetResources();
    bool TryReduceResources(IDictionary<string, int> resourceAmounts);
    void ReduceResources(IDictionary<string, int> resourceAmounts);
    bool CanReduceResources(IDictionary<string, int> resourceAmounts);
    bool TryIncreaseResources(IDictionary<string, int> resourceAmounts);
    void IncreaseResources(IDictionary<string, int> resourceAmounts);
    bool CanIncreaseResources(IDictionary<string, int> resourceAmounts);

}
