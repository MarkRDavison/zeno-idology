namespace Idology.Outpost.Core.Data;

public sealed class Zombie
{
    public Vector2 Position { get; set; }
    public Vector2? TargetPosition { get; set; }
    public float IdleTime { get; set; }
    public ZombieMode Mode { get; set; } = ZombieMode.Wandering;
}
