namespace Idology.Conservation.Core.Services.Panel;

public interface IScreenPanelService
{
    bool ClosePanel(ScreenPanelState panelState);
    bool OpenPanel(ScreenPanelState panelState);
}
