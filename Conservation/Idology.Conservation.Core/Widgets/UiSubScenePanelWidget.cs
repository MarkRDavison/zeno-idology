namespace Idology.Conservation.Core.Widgets;

public abstract class UiSubScenePanelWidget : PanelWidget
{

    protected UiSubScenePanelWidget(
        ITranslationService translationService,
        IGameCommandService gameCommandService)
    {
        TranslationService = translationService;
        GameCommandService = gameCommandService;

        Background = Color.Gray;
        Border = Color.DarkGray;
        BorderThickness = 2.0f;
        Layout.Contain = ContainFlags.Column;
        Layout.Behave = BehaveFlags.Fill;
        Layout.RequestedPadding = new LayoutEdges(16.0f);
        Layout.RequestedMargin = new LayoutEdges(32.0f);
    }

    public abstract string TitleTranslationKey { get; }
    public ITranslationService TranslationService { get; }
    public IGameCommandService GameCommandService { get; }

    protected ScrollablePanelWidget AddCommonWidgets(Action<BaseWidget>? postTitleContentAction = null)
    {
        var headerRow = AddChild(new PanelWidget
        {
            Layout =
            {
                Behave = BehaveFlags.HFill,
                Align = AlignFlags.Start,
                Contain = ContainFlags.Row
            }
        });

        headerRow.AddChild(new LabelWidget
        {
            TextContent = TranslationService[TitleTranslationKey],
            Foreground = Color.White,
            Layout =
            {
                RequestedMargin = new LayoutEdges(4.0f, 0.0f, 0.0f, 0.0f),
                Behave = BehaveFlags.Left | BehaveFlags.HFill | BehaveFlags.Top,
                RequestedSize = new LayoutVector(0, 36)
            }
        });

        headerRow.AddChild(new SpacerWidget(ContainFlags.Row));

        if (postTitleContentAction is not null)
        {
            postTitleContentAction(headerRow);
        }

        var closePanelButton = headerRow.AddChild(new IconButtonWidget
        {
            Foreground = Color.White,
            IconTextureName = "icon-x-24", // TODO: Constant...
            Layout = new LayoutItem
            {
                RequestedPadding = new LayoutEdges(2.0f),
                RequestedSize = new LayoutVector(32, 32),
                Contain = ContainFlags.Row,
                Behave = BehaveFlags.Top | BehaveFlags.Right
            }
        });

        closePanelButton.OnClick += (s, e) => GameCommandService.HandleCommand(new SetScreenStateGameCommand { ScreenState = ScreenState.Default });

        return AddChild(new ScrollablePanelWidget
        {
            Background = Color.Gray,
            Border = Color.DarkGray,
            BorderThickness = 2.0f,
            Layout =
            {
                Gap = 2.0f,
                Behave = BehaveFlags.Fill,
                Contain = ContainFlags.Column,
                Align = AlignFlags.Start,
                RequestedPadding = new LayoutEdges(4.0f)
            }
        });
    }
}
