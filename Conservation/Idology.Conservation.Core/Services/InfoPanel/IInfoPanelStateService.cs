namespace Idology.Conservation.Core.Services.InfoPanel;

public interface IInfoPanelStateService
{
    void PopInfoPanel(InfoState infoState);
    void PushInfoPanel(InfoState infoState, object? payload);
}
