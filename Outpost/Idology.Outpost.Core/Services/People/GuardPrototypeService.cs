namespace Idology.Outpost.Core.Services.People;

public sealed class GuardPrototypeService : PrototypeService<GuardPrototype, Guard>
{
    public override Guard CreateEntity(GuardPrototype prototype)
    {
        return new Guard
        {
            Id = Guid.NewGuid(),
            PrototypeId = prototype.Id,
            Class = prototype.Name
        };
    }
}
