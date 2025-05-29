namespace Idology.Core.Services;

public sealed class BuildingPrototypeService : PrototypeService<BuildingPrototype, BuildingComponent>
{
    public override BuildingComponent CreateEntity(BuildingPrototype prototype)
    {
        return new BuildingComponent
        {
            Id = Guid.NewGuid(),
            PrototypeId = prototype.Id,
            Accomodation = prototype.ProvidedAccomodation.ToDictionary(_ => _.Key, _ => new AmountRange
            {
                Min = 0,
                Current = 0,
                Max = _.Value
            })
        };
    }
}
