namespace Idology.Conservation.Core.Scenes.SubScenes;

internal sealed record StaffDetailsSubScenePayload();

// TODO: What is the point of these now?
internal sealed class StaffDetailsSubScene : SubScene<StaffDetailsSubScene, StaffDetailsSubScenePayload>
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    public StaffDetailsSubScene(
        ConservationGameData gameData,
        IGameDateTimeProvider gameDateTimeProvider,
        IServiceProvider serviceProvider) : base(
        gameData)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    public override void Init(StaffDetailsSubScenePayload? payload)
    {
    }

}
