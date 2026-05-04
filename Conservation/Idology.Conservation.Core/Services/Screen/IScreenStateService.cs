namespace Idology.Conservation.Core.Services.Screen;

public interface IScreenStateService
{
    bool CloseScreen(ScreenState screenState);
    bool OpenScreenState(ScreenState screenState);
}
