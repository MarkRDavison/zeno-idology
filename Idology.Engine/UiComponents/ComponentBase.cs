namespace Idology.Engine.UiComponents;

public abstract class ComponentBase
{
    public static IServiceProvider Services { get; set; } = null!;
    public virtual void Update(float delta)
    {

    }

    public abstract void Draw();
}
