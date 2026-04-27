namespace Idology.Conservation.Core.Widgets;

public sealed class StaffDetailsUiSubScenePanelWidget : UiSubScenePanelWidget
{
    public StaffDetailsUiSubScenePanelWidget(
        ITranslationService translationService
    ) : base(
        translationService)
    {
    }

    public override string TitleTranslationKey => "STAFF_DETAILS_TITLE";

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets();
    }
}
