namespace Idology.Conservation.Core.Widgets;

public abstract class UiSubScenePanelWidget : PanelWidget
{

    protected UiSubScenePanelWidget(ITranslationService translationService)
    {
        TranslationService = translationService;

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

    protected ScrollablePanelWidget AddCommonWidgets()
    {
        AddChild(new LabelWidget
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
