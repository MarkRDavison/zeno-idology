namespace Idology.Conservation.Core.Scenes.SubScenes;

public abstract class SubScene
{
    protected SubScene(ConservationGameData gameData)
    {
        GameData = gameData;
    }

    public virtual void Update(float delta)
    {

    }

    public virtual void Draw(Camera2D camera)
    {

    }

    public virtual void OnWindowResize(int width, int height)
    {

    }

    protected ConservationGameData GameData { get; }
}

// TODO: Add another generic param so you can get a specific typed payload?
public abstract class SubScene<TScene, TSubScenePayload> : SubScene where TScene : SubScene where TSubScenePayload : class
{
    protected SubScene(ConservationGameData gameData) : base(gameData)
    {
    }

    public virtual void Init(TSubScenePayload? payload)
    {

    }
}
