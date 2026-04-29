namespace Idology.Conservation.Core.Widgets;

public sealed class TechnologyUiSubScenePanelWidget : UiSubScenePanelWidget
{
    public TechnologyUiSubScenePanelWidget(
        ITranslationService translationService,
        IGameCommandService gameCommandService
    ) : base(
        translationService,
        gameCommandService)
    {
    }

    public override string TitleTranslationKey => "TECHNOLOGY_DETAILS_TITLE";

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets();
    }
}
