namespace Idology.Core.Components;

public class BuildingComponent : IEntity
{
    public Guid Id { get; set; }
    public Guid PrototypeId { get; set; }
    public Dictionary<Guid, AmountRange> Accomodation { get; set; } = [];
}
