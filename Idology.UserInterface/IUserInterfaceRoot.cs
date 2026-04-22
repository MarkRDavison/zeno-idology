namespace Idology.UserInterface;

public interface IUserInterfaceRoot
{
    void Update(float delta);
    void SetBounds(LayoutVector bounds);

    IWidget RootWidget { get; }
}
