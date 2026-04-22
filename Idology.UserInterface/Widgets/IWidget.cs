namespace Idology.UserInterface.Widgets;

public interface IWidget
{
    LayoutItem Layout { get; init; }

    Color Background { get; set; }
    Color Foreground { get; set; }
    Color Border { get; set; }
    float? BorderRoundness { get; set; }

    TWidget AddChild<TWidget>(TWidget child) where TWidget : BaseWidget;

    void Update(float delta);
    void Draw();
}
