namespace Idology.Conservation.Core.Services.InfoPanel;

public interface IInfoPanelStateService
{
    void PushInfoPanel(InfoState infoState, object? payload);
}
