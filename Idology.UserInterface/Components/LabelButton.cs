namespace Idology.UserInterface.Components;

public class LabelButton : ButtonBase
{
    public string Label { get; set; } = string.Empty;
    public int FontSize { get; set; } = 32;

    public override Rectangle Draw()
    {
        var bounds = base.Draw();

        var textSize = Raylib.MeasureText(Label, FontSize);

        var decrease = 0;
        var newLabel = Label;
        const string Ellipses = "...";
        while (textSize > Size.X - ButtonBorderSize * 2)
        {
            decrease++;
            newLabel = Label.Substring(0, Label.Length - Ellipses.Length - decrease) + Ellipses;
            textSize = Raylib.MeasureText(newLabel, FontSize);
        }

        Raylib.DrawText(
            newLabel,
            (int)(Position.X + Size.X / 2 - textSize / 2),
            (int)(Position.Y + Size.Y / 2 - FontSize / 2),
            FontSize,
            Hovered ? Color.White : Color.Black);

        return bounds;
    }
}
