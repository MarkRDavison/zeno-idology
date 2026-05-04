namespace Idology.Conservation.Core.Widgets;

public sealed class ResearchUiSubScenePanelWidget : UiSubScenePanelWidget
{
    public ResearchUiSubScenePanelWidget(
        ITranslationService translationService,
        IGameCommandService gameCommandService
    ) : base(
        translationService,
        gameCommandService)
    {
    }

    public override string TitleTranslationKey => "RESEARCH_DETAILS_TITLE";
    public override ScreenState ScreenState => ScreenState.Research;

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets(_ =>
        {

        });
    }
}
