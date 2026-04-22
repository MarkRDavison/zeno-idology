namespace Idology.UserInterface;

internal sealed class UserInterfaceRoot : IUserInterfaceRoot
{
    private LayoutVector _bounds;
    private IWidget? _rootWidget;

    private readonly IInputManager _inputManager;

    public UserInterfaceRoot(IInputManager inputManager)
    {
        _bounds = new();
        _inputManager = inputManager;
    }

    public void Update(float delta)
    {
        // TODO: Which order?
        RootWidget.Update(delta);
        RootWidget.Layout.Run();
    }

    public void SetBounds(LayoutVector bounds)
    {
        _bounds = bounds;

        RootWidget.Layout.RequestedSize = new LayoutVector(
            _bounds.X - RootWidget.Layout.RequestedMargin.Left - RootWidget.Layout.RequestedMargin.Right,
            _bounds.Y - RootWidget.Layout.RequestedMargin.Top - RootWidget.Layout.RequestedMargin.Bottom);
    }

    public IWidget RootWidget
    {
        get
        {
            _rootWidget ??= new RootWidget
            {
                InputManager = _inputManager,
                Layout = new LayoutItem
                {
                    RequestedSize = _bounds,
                    Contain = ContainFlags.Layout
                }
            };

            return _rootWidget;
        }
    }
}
