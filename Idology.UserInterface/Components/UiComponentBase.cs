namespace Idology.UserInterface.Components;

public abstract class UiComponentBase
{
    public required ITextureManager TextureManager { get; set; }
    public virtual void Update(float delta) { }
    public virtual Rectangle Measure() => new();
    public virtual Rectangle Draw() => new();
}
