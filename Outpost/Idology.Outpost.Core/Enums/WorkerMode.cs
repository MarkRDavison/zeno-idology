namespace Idology.Outpost.Core.Enums;

public enum WorkerMode
{
    TravellingToWork,
    Working,
    ReturningResources,
    ReturningHome,

    Returning = ReturningHome | ReturningResources
}
