namespace Idology.Engine.Core;

public abstract class Scene
{
    public virtual void Update(float delta)
    {

    }

    public virtual void Draw()
    {

    }
}

// TODO: Add another generic param so you can get a specific typed payload?
public abstract class Scene<TScene> : Scene where TScene : Scene
{
    public virtual void Init(IScenePayload<TScene>? payload)
    {

    }
}
