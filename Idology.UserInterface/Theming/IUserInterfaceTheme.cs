namespace Idology.UserInterface.Theming;

public interface IUserInterfaceTheme
{
    int BorderSize { get; }
    Color BorderColor { get; }
    Color ForegroundColor { get; }
    Color InnerColor { get; }
    Color HoverForegroundColor { get; }

}
