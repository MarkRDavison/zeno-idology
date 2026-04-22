namespace Idology.UserInterface.Widgets;

public sealed class PanelWidget : BaseWidget
{

    public override void Draw()
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


        DrawChildren();
    }
}
