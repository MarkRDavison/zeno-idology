namespace Idology.Conservation.Core.Widgets;

public sealed class KakapoDetailsUiSubScenePanelWidget : UiSubScenePanelWidget
{
    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public KakapoDetailsUiSubScenePanelWidget(
        ITranslationService translationService,
        IGameDateTimeProvider gameDateTimeProvider
    ) : base(
        translationService)
    {
        _gameDateTimeProvider = gameDateTimeProvider;
    }

    public override string TitleTranslationKey => "KAKAPO_DETAILS_TITLE";

    public override void PostConstructInit()
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

        var scrollableWidget = AddChild(new ScrollablePanelWidget
        {
            Background = Color.Gray,
            Border = Color.DarkGray,
            BorderThickness = 2.0f,
            Layout =
            {
                Behave = BehaveFlags.Fill,
                Contain = ContainFlags.Column,
                Align = AlignFlags.Start,
                RequestedPadding = new LayoutEdges(4.0f)
            }
        });

        for (int i = 0; i < 8; ++i)
        {
            var currentPanel = scrollableWidget.AddChild(new PanelWidget
            {
                Background = Color.Red,
                Border = Color.Magenta,
                BorderThickness = 4.0f,
                Layout =
                {
                    Behave = BehaveFlags.HFill,
                    Contain = ContainFlags.Flex,
                    ItemFlags = ItemFlags.VFixed,
                    RequestedSize = new LayoutVector(0, 128)
                }
            });

            currentPanel.AddChild(new LabelWidget
            {
                Foreground = Color.SkyBlue,
                TextContent = $"Scrollable #{i + 1}",
                FontSize = 64
            });
        }
    }
}
