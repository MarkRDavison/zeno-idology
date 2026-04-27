namespace Idology.Conservation.Core.Widgets;

public sealed class ResearchUiSubScenePanelWidget : UiSubScenePanelWidget
{
    public ResearchUiSubScenePanelWidget(
        ITranslationService translationService
    ) : base(
        translationService)
    {
    }

    public override string TitleTranslationKey => "RESEARCH_DETAILS_TITLE";

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets();
    }
}
