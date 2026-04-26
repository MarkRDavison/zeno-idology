namespace Idology.Conservation.Core.Scenes;

public abstract class ConservationScene<TScene> : Scene<TScene> where TScene : Scene
{
    private readonly Stack<SubScene> _subScenes = [];

    protected void PushSubScene(SubScene subScene)
    {
        _subScenes.Push(subScene);
    }

    protected void PopSubScene()
    {
        if (_subScenes.Count is 0)
        {
            throw new InvalidOperationException("Cannot pop empty sub scene stack");
        }

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
