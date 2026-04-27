namespace Idology.Conservation.Core.Widgets;

public sealed class FundingUiSubScenePanelWidget : UiSubScenePanelWidget
{
    public FundingUiSubScenePanelWidget(
        ITranslationService translationService
    ) : base(
        translationService)
    {
    }

    public override string TitleTranslationKey => "FUNDING_DETAILS_TITLE";

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets();
    }
}
