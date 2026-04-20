namespace Idology.Engine.Core;

public sealed class SceneService : ISceneService
{
    private readonly Application _application;
    private readonly IServiceProvider _serviceProvider;

    public SceneService(
        Application application,
        IServiceProvider serviceProvider)
    {
        _application = application;
        _serviceProvider = serviceProvider;
    }

    public TScene SetScene<TScene>(IScenePayload<TScene>? payload) where TScene : Scene
    {
        // TODO: DEFER
        var scene = _serviceProvider.GetRequiredService<TScene>();

        if (scene is Scene<TScene> initableScene)
        {
            initableScene.Init(payload);
        }

        _application.SetScene(scene);

        return scene;
    }

    public void Init()
    {
        throw new NotImplementedException();
    }
}
