namespace Idology.Conservation.Core.Widgets;

public sealed class FundingUiSubScenePanelWidget : UiSubScenePanelWidget
{
    public FundingUiSubScenePanelWidget(
        ITranslationService translationService,
        IGameCommandService gameCommandService
    ) : base(
        translationService,
        gameCommandService)
    {
    }

    public override string TitleTranslationKey => "FUNDING_DETAILS_TITLE";

    public override void PostConstructInit()
    {
        var scrollableWidget = AddCommonWidgets(_ =>
        {

        });
    }
}
