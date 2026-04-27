namespace Idology.UserInterface.Widgets;

public class ScrollablePanelWidget : PanelWidget
{
    public override void Draw()
    {
        if (Parent?.Layout is { } parentLayout)
        {
            //Raylib.BeginScissorMode((int)Layout.Rect.X, (int)Layout.Rect.Y, (int)parentLayout.Rect.Width, (int)parentLayout.Rect.Height);

            base.Draw();

            //Raylib.EndScissorMode();
        }
    }
}
