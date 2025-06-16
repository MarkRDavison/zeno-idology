namespace Idology.Space.Core.Services;

public interface IInputService
{
    void Update(float delta);
    bool IsMouseDoubleClicked(MouseButton button);
}
