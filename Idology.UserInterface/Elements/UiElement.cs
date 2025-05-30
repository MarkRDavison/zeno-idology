namespace Idology.UserInterface.Elements;

public abstract class UiElement
{
    public string Name { get; set; } = Guid.NewGuid().ToString();
    public float Width { get; set; }
    public float Height { get; set; }
    public float MinWidth { get; set; }
    public float MinHeight { get; set; }
    public float MaxWidth { get; set; }
    public float MaxHeight { get; set; }
    public float ChildGap { get; set; }
    public LayoutDirection LayoutDirection { get; set; } = LayoutDirection.LeftToRight;
    public UiElement? Parent { get; set; }
    public List<UiElement> Children { get; set; } = [];
    public Padding Padding { get; set; } = new();
    public Sizing WidthSizing { get; set; } = Sizing.Fit();
    public Sizing HeightSizing { get; set; } = Sizing.Fit();
    public List<float> Colour { get; set; } = [1.0f, 0.0f, 0.0f, 1.0f];
}
