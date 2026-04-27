namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record FundingSubScenePayload();

// TODO: What is the point of these now?
internal sealed class FundingSubScene : SubScene<FundingSubScene, FundingSubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    public FundingSubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(FundingSubScenePayload? payload)
    {

    }
}
