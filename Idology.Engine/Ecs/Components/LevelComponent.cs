namespace Idology.Engine.Ecs.Components;

public sealed class LevelComponent
{
    public Dictionary<Guid, int> TotalJobs { get; set; } = [];
    public Dictionary<Guid, int> TotalAccomodation { get; set; } = [];
    public Dictionary<Guid, int> TotalWorkers { get; set; } = [];
}

public sealed class LevelChunkComponent
{
    public Vector2 Coordinates { get; set; }
}