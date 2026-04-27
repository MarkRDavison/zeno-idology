namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record ResearchSubScenePayload();

// TODO: What is the point of these now?
internal sealed class ResearchSubScene : SubScene<ResearchSubScene, ResearchSubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    public ResearchSubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(ResearchSubScenePayload? payload)
    {
    }
}
