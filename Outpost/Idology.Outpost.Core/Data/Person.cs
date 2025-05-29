namespace Idology.Outpost.Core.Data;

public sealed class Person
{
    public WorkerMode Mode { get; set; } = WorkerMode.TravellingToWork;
    public bool RequiresRemoval { get; set; }
    public string Class { get; set; } = string.Empty;
    public Vector2 Position { get; set; }
    public Vector2 TargetPosition { get; set; }
    public Queue<Vector2> Waypoints { get; } = [];
    public float ElapsedWork { get; set; }
}
