namespace Idology.Conservation.Core.Widgets;

public class SpacerWidget : PanelWidget
{
    public SpacerWidget()
    {
        Layout.RequestedSize = new LayoutVector(0, 0);
        Layout.Behave = BehaveFlags.Fill;
    }
}
