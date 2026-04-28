namespace Idology.Conservation.Core.Widgets;

public sealed class KakapoDetailsUiSubScenePanelWidget : UiSubScenePanelWidget
{
    private readonly ConservationGameData _gameData;
    private readonly IGameDateTimeProvider _gameDateTimeProvider;

    public KakapoDetailsUiSubScenePanelWidget(
        ConservationGameData gameData,
        ITranslationService translationService,
        IGameDateTimeProvider gameDateTimeProvider
    ) : base(
        translationService)
    {
        _gameData = gameData;
        _gameDateTimeProvider = gameDateTimeProvider;
    }

    public override string TitleTranslationKey => "KAKAPO_DETAILS_TITLE";

    // TODO: localization...
    public override void PostConstructInit()
    {
        // Add ability to add content after the title, (number of birds, filtering options etc)
        var scrollableWidget = AddCommonWidgets();

        foreach (var kd in _gameData.KakapoData)
        {
            var rootRowPanel = scrollableWidget.AddChild(new PanelWidget
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                BorderThickness = 2.0f,
                Layout =
                {
                    RequestedPadding = new LayoutEdges(8.0f),
                    RequestedMargin = new LayoutEdges(0.0f, 0.0f, 0.0f, 2.0f), // TODO: REPLACE WITH PARENT GAP
                    Behave = BehaveFlags.HFill,
                    Contain = ContainFlags.Column,
                    ItemFlags = ItemFlags.VFixed
                }
            });

            {
                var primaryRowPanel = rootRowPanel.AddChild(new PanelWidget
                {
                    Layout =
                    {
                        Behave = BehaveFlags.Fill,
                        Contain = ContainFlags.Row,
                        Align = AlignFlags.Start
                    }
                });

                var ageInfo = string.Empty; // TODO: Calculate age based on birth/death dates, different if discovered not born

                primaryRowPanel.AddChild(new LabelWidget
                {
                    Foreground = Color.White,
                    TextContent = $"{kd.Name} {ageInfo}",
                    FontSize = 24,
                    Layout =
                    {
                        Align = AlignFlags.Start,
                        RequestedMargin = new LayoutEdges(4.0f),
                        Behave = BehaveFlags.VCenter | BehaveFlags.Left,
                        RequestedSize = new LayoutVector(0, 24)
                    }
                });
            }
            {
                var secondaryColumnPanel = rootRowPanel.AddChild(new PanelWidget
                {
                    Layout =
                    {
                        Behave = BehaveFlags.Fill,
                        Contain = ContainFlags.Column,
                        Align = AlignFlags.Start
                    }
                });

                static string GetBirthDiscoveryInfo(KakapoData kakapo)
                {
                    if (kakapo.Origin.Date is null)
                    {
                        return string.Empty;
                    }

                    return kakapo.Origin.Type switch
                    {
                        OriginDateType.KnownBirth => $"Born {kakapo.Origin.Date.Value:d}",
                        OriginDateType.EstimatedBirth => $"Born in {kakapo.Origin.Date.Value:yyyy}",
                        OriginDateType.Discovered => $"Discovered in {kakapo.Origin.Date.Value:yyyy}",
                        _ => string.Empty
                    };
                }

                if (GetBirthDiscoveryInfo(kd) is { } birthDiscoveryInfo && !string.IsNullOrEmpty(birthDiscoveryInfo))
                {
                    var deathInfo = kd.Death is null
                        ? string.Empty
                        : $", died in {kd.Death.Value:yyyy}";

                    secondaryColumnPanel.AddChild(new LabelWidget
                    {
                        Foreground = kd.Death is null ? Color.Green : Color.Red,
                        // TODO: Instead of directly setting the text create a binding/func<string>?
                        TextContent = "- " + birthDiscoveryInfo + deathInfo,
                        FontSize = 20,
                        Layout =
                        {
                            Align = AlignFlags.Start,
                            RequestedMargin = new LayoutEdges(4.0f),
                            Behave = BehaveFlags.VCenter | BehaveFlags.Left,
                            RequestedSize = new LayoutVector(0, 24)
                        }
                    });
                }

                if (kd.Origin.Type is OriginDateType.KnownBirth or OriginDateType.EstimatedBirth)
                {
                    var parentageInfo = string.Empty;

                    if (kd.MotherId is not null && _gameData.KakapoData.FirstOrDefault(_ => _.Id == kd.MotherId) is { } mother)
                    {
                        parentageInfo += "Mother " + mother.Name;
                    }
                    else
                    {
                        parentageInfo += "Mother unknown";
                    }

                    parentageInfo += ", ";

                    if (kd.FatherId is not null && _gameData.KakapoData.FirstOrDefault(_ => _.Id == kd.FatherId) is { } father)
                    {
                        parentageInfo += "father " + father.Name;
                    }
                    else
                    {
                        parentageInfo += "father unknown";
                    }

                    secondaryColumnPanel.AddChild(new LabelWidget
                    {
                        Foreground = Color.White,
                        // TODO: Instead of directly setting the text create a binding/func<string>?
                        TextContent = "- " + parentageInfo,
                        FontSize = 20,
                        Layout =
                        {
                            Align = AlignFlags.Start,
                            RequestedMargin = new LayoutEdges(4.0f),
                            Behave = BehaveFlags.VCenter | BehaveFlags.Left,
                            RequestedSize = new LayoutVector(0, 24)
                        }
                    });
                }


                if (!string.IsNullOrEmpty(kd.Origin.Notes))
                {
                    secondaryColumnPanel.AddChild(new LabelWidget
                    {
                        Foreground = Color.White,
                        // TODO: Instead of directly setting the text create a binding/func<string>?
                        TextContent = "- " + kd.Origin.Notes,
                        FontSize = 20,
                        Layout =
                        {
                            Align = AlignFlags.Start,
                            RequestedMargin = new LayoutEdges(4.0f),
                            Behave = BehaveFlags.VCenter | BehaveFlags.Left,
                            RequestedSize = new LayoutVector(0, 24)
                        }
                    });
                }
            }
        }
    }
}
