namespace Idology.Engine.Core;

public interface ISceneService
{
    TScene SetScene<TScene>(IScenePayload<TScene>? payload) where TScene : Scene;

    void Init();
}
