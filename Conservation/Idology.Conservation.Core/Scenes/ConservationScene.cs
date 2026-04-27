namespace Idology.Conservation.Core.Scenes;

public abstract class ConservationScene<TScene> : Scene<TScene> where TScene : Scene
{
    private readonly Stack<SubScene> _subScenes = [];

    protected TSubScene PushSubScene<TSubScene>(TSubScene subScene) where TSubScene : SubScene
    {
        _subScenes.Push(subScene);

        return subScene;
    }

    protected void PopAllScenes()
    {
        while (_subScenes.Count > 0)
        {
            PopSubScene();
        }
    }

    protected void PopSubScene()
    {
        if (_subScenes.Count is 0)
        {
            throw new InvalidOperationException("Cannot pop empty sub scene stack");
        }

        _subScenes.Peek().OnPopped();
        _subScenes.Pop();
    }

    protected void ForEachSubSceneReverse(Action<SubScene> action)
    {
        foreach (var ss in _subScenes.Reverse())
        {
            action(ss);
        }
    }

    protected void ForEachSubScene(Action<SubScene> action)
    {
        foreach (var ss in _subScenes)
        {
            action(ss);
        }
    }
}
