namespace Idology.Conservation.Core.Services.Kakapo;

public interface IKakapoStateService
{
    void OpenKakapoScreenState();
    void SetActiveKakapoId(int kakapoId);
    void SetInfoPanelToKakapo();
}
