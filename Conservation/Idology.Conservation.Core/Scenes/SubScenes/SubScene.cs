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

    public virtual void Draw()
    {

    }

    protected ConservationGameData GameData { get; }
}
