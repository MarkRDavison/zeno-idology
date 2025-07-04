﻿namespace Idology.Outpost.Core.Data.Prototypes;

public sealed class ZombiePrototype : IPrototype
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Damage { get; set; }
    public int Health { get; set; }
}
