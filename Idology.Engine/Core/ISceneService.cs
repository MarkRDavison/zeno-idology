namespace Idology.Engine.Core;

public interface ISceneService
{
    void SetScene<TScene>() where TScene : Scene;

    void Init();
}
