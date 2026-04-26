namespace Idology.UserInterface.Widgets;

public abstract class BaseWidget : IWidget
{
    private readonly List<IWidget> _children = [];

    public IWidget? Parent { get; private set; }
    public LayoutItem Layout { get; init; } = new();
    public Color Background { get; set; } = Color.Blank;
    public Color Foreground { get; set; } = Color.Blank;
    public Color Border { get; set; } = Color.Blank;

    // TODO: Should these all be float? or float...

    public float? BorderRoundness { get; set; }
    public float? BorderThickness { get; set; }

    public TWidget AddChild<TWidget>(TWidget child) where TWidget : BaseWidget
    {
        if (child is BaseWidget bw)
        {
            bw.InputManager = InputManager;
        }

        // TODO: Need to keep layout/widget in sync when adding/removing...
        Layout.AddChild(child.Layout);
        _children.Add(child);
        child.Parent = this;

        child.PostConstructInit();

        return child;
    }

    protected bool LayoutBoundsContainMousePosition()
    {
        var mouse = InputManager.GetMousePosition();

        if (Layout.Rect.X <= mouse.X && mouse.X <= Layout.Rect.X + Layout.Rect.Width &&
            Layout.Rect.Y <= mouse.Y && mouse.Y <= Layout.Rect.Y + Layout.Rect.Height)
        {
            return true;
        }

        return false;
    }
    public virtual void PostConstructInit()
    {

    }

    public virtual void Update(float delta)
    {
        UpdateChildren(delta);
    }

    public virtual void Draw()
    {
        DrawChildren();
    }

    protected void UpdateChildren(float delta)
    {
        foreach (var c in _children)
        {
            c.Update(delta);
        }
    }

    protected void DrawChildren()
    {
        foreach (var c in _children)
        {
            c.Draw();
        }
    }

    public IInputManager InputManager { get; set; } = default!;
}
