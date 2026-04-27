namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record KakapoDetailsSubScenePayload();

// TODO: What is the point of these now?
internal sealed class KakapoDetailsSubScene : SubScene<KakapoDetailsSubScene, KakapoDetailsSubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    public KakapoDetailsSubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(KakapoDetailsSubScenePayload? payload)
    {
    }
}
