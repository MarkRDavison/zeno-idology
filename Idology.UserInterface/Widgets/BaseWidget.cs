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

    public void AddGenericChild(BaseWidget child)
    {
        // TODO: Need to keep layout/widget in sync when adding/removing...
        Layout.AddChild(child.Layout);
        _children.Add(child);
        child.Parent = this;

        child.PostConstructInit();
    }

    public TWidget AddChild<TWidget>(TWidget child) where TWidget : BaseWidget
    {
        child.InputManager = InputManager;
        child.UserInterfaceRoot = UserInterfaceRoot;

        AddGenericChild(child);

        return child;
    }

    protected void ClearChildren()
    {
        foreach (var c in Layout.Children)
        {
            c.Reset(true);
        }

        _children.Clear();
        Layout.FirstChild = null;
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
            if (c.Layout.Visibility is Visibility.Visible)
            {
                c.Draw();
            }
        }
    }

    public void ForEachChildRecursively(Action<IWidget> action)
    {
        foreach (var c in _children)
        {
            action(c);
            c.ForEachChildRecursively(action);
        }
    }

    public IInputManager InputManager { get; set; } = default!;
    public IUserInterfaceRoot UserInterfaceRoot { get; set; } = default!;
}
