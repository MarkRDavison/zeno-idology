namespace Idology.Outpost.Core.Data.Entities;

public sealed class Zombie : IEntity
{
    public Guid Id { get; set; }
    public Guid PrototypeId { get; set; }
    public Vector2 Position { get; set; }
    public Vector2? TargetPosition { get; set; }
    public Vector2? TargetRegion { get; set; }
    public float IdleTime { get; set; }
    public ZombieMode Mode { get; set; } = ZombieMode.Wandering;
    public int Damage { get; set; }
}
