namespace Idology.Space.Core.Services;

public class InputService : IInputService
{
    const double DoubleClickThreshold = 0.4;

    private readonly Dictionary<MouseButton, DateTime> _lastPressed = [];
    // TODO: DateTimeService

    public InputService()
    {

        foreach (var button in Enum.GetValues(typeof(MouseButton)).OfType<MouseButton>())
        {
            _lastPressed[button] = DateTime.MinValue;
        }
    }

    public void Update(float delta)
    {
        foreach (var button in Enum.GetValues(typeof(MouseButton)).OfType<MouseButton>())
        {
            if (Raylib.IsMouseButtonPressed(button))
            {
                _lastPressed[button] = DateTime.Now;
            }
        }
    }

    public bool IsMouseDoubleClicked(MouseButton button)
    {
        if (Raylib.IsMouseButtonPressed(button))
        {
            if (_lastPressed.TryGetValue(button, out var buttonLastPressedAt))
            {
                _lastPressed[button] = DateTime.Now;

                if ((DateTime.Now - buttonLastPressedAt).TotalSeconds <= DoubleClickThreshold)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
