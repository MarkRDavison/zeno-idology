namespace Idology.Outpost.Core.Services.Zombies;

public sealed class ZombiePrototypeService : PrototypeService<ZombiePrototype, Zombie>
{
    public override Zombie CreateEntity(ZombiePrototype prototype)
    {
        return new Zombie
        {
            Id = Guid.NewGuid(),
            PrototypeId = prototype.Id,
            IdleTime = 0.0f,
            Mode = ZombieMode.Wandering,
            Damage = prototype.Damage
        };
    }
}
