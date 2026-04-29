namespace Idology.Conservation.Core.Widgets;

public class SpacerWidget : PanelWidget
{
    public SpacerWidget(ContainFlags containFlags)
    {
        Layout.RequestedSize = new LayoutVector(0, 0);
        Layout.Behave = BehaveFlags.Fill;
        Layout.Contain = containFlags;
    }
}
