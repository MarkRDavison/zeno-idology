namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record TechnologySubScenePayload();

// TODO: What is the point of these now?
internal sealed class TechnologySubScene : SubScene<TechnologySubScene, TechnologySubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    public TechnologySubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(TechnologySubScenePayload? payload)
    {
    }
}
