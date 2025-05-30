namespace Idology.Outpost.Core.Services.People;

public sealed class PersonPrototypeService : PrototypeService<PersonPrototype, Person>
{
    public override Person CreateEntity(PersonPrototype prototype)
    {
        return new Person
        {
            Id = Guid.NewGuid(),
            PrototypeId = prototype.Id,
            Class = prototype.Name,
            Inventory = prototype.Inventory.ToDictionary(_ => _.Key, _ => _.Value.Clone())
        };
    }
}
