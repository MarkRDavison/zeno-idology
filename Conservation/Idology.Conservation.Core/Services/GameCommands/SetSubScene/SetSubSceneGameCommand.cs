namespace Idology.Conservation.Core.Services.GameCommands.SetSubScene;

public sealed class SetSubSceneGameCommand : IGameCommand
{
    public required string Id { get; init; }
}