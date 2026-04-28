namespace Idology.UserInterface.Widgets;

public class PanelWidget : BaseWidget
{
    protected void DrawPanelSelf()
    {
        const int Segments = 8;

        // TODO: ROUNDED-NESS
        //Raylib.DrawRectangleRoundedLinesEx

        if (BorderRoundness is not null && BorderRoundness > 0.0f)
        {
            Raylib.DrawRectangleRoundedLinesEx(
                new Rectangle(
                    (int)Layout.Rect.X,
                    (int)Layout.Rect.Y,
                    (int)Layout.Rect.Width,
                    (int)Layout.Rect.Height),
                BorderRoundness.Value,
                Segments,
                (int)(BorderThickness ?? 0),
                Border);

            Raylib.DrawRectangleRounded(
                new Rectangle(
                    (int)Layout.Rect.X,
                    (int)Layout.Rect.Y,
                    (int)Layout.Rect.Width,
                    (int)Layout.Rect.Height),
                BorderRoundness.Value,
                Segments,
                Background);
        }
        else
        {
            if (BorderThickness.GetValueOrDefault() > 0)
            {
                Raylib.DrawRectangleLinesEx(
                    new Rectangle(
                        (int)Layout.Rect.X,
                        (int)Layout.Rect.Y,
                        (int)Layout.Rect.Width,
                        (int)Layout.Rect.Height),
                    (int)(BorderThickness ?? 0),
                    Border);
            }

            Raylib.DrawRectangle(
                (int)Layout.Rect.X + (int)(BorderThickness ?? 0),
                (int)Layout.Rect.Y + (int)(BorderThickness ?? 0),
                (int)Layout.Rect.Width - (int)(BorderThickness ?? 0) * 2,
                (int)Layout.Rect.Height - (int)(BorderThickness ?? 0) * 2,
                Background);
        }
    }

    public override void Draw()
    {
        DrawPanelSelf();

        // Debug: if a panel has a red background, log its and its descendants' layout rects
        // This helps diagnose cases where a child appears centered at runtime.
        if (Background.Equals(Color.Red))
        {
            System.Console.WriteLine($"DEBUG PanelWidget ({GetType().Name}) rect={Layout.Rect}");
            ForEachChildRecursively(c =>
            {
                System.Console.WriteLine($"DEBUG Child ({c.GetType().Name}) rect={c.Layout.Rect}");
            });
        }

        DrawChildren();
    }
}
