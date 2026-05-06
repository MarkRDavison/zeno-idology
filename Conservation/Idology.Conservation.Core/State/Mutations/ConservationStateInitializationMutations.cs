namespace Idology.Conservation.Core.State.Mutations;

public static class ConservationStateInitializationMutations
{
    public static ConservationGameData CreateDefaultData()
    {
        return new ConservationGameData(
            new ConservationInteractionData(
                [],
                MainScreenState.Default,
                ScreenPanelState.None,
                new DefaultScreenData(
                    null),
                new RegionScreenData(
                    null,
                    null)),
            null,
            [],
            [],
            [],
            [],
            []);
    }

    public static ConservationGameData WithSetKakapoData(
        this ConservationGameData state,
        IList<KakapoModel> kakapo)
    {
        return state with
        {
            KakapoData = [.. kakapo]
        };
    }

    public static ConservationGameData WithSetStaffData(
        this ConservationGameData state,
        IList<StaffData> staff)
    {
        return state with
        {
            StaffData = [.. staff]
        };
    }

    public static ConservationGameData WithSetResearchData(
        this ConservationGameData state,
        IList<ResearchData> research)
    {
        return state with
        {
            ResearchData = [.. research]
        };
    }

    public static ConservationGameData WithSetRegionData(
        this ConservationGameData state,
        IList<RegionData> regions)
    {
        return state with
        {
            Regions = [.. regions]
        };
    }

    public static ConservationGameData WithSetKakapoSimulations(
        this ConservationGameData state,
        IList<KakapoSimulationData> kakapoSimulations)
    {
        return state with
        {
            SimulatedKakapo = [.. kakapoSimulations]
        };
    }

    public static ConservationGameData WithUpdatedKakapoSimulations(
        this ConservationGameData state,
        IList<KakapoSimulationData> kakapoSimulations)
    {
        var updatedKakapoIds = kakapoSimulations.Select(_ => _.KakapoId).ToHashSet();

        return state with
        {
            SimulatedKakapo =
            [
                .. state.SimulatedKakapo.Where(_ => !updatedKakapoIds.Contains(_.KakapoId)),
                .. kakapoSimulations
            ]
        };
    }

    public static ConservationGameData WithInteractionScreenState(
        this ConservationGameData state,
        MainScreenState screenState)
    {
        return state with
        {
            InteractionData = state.InteractionData with
            {
                MainScreenState = screenState
            }
        };
    }

    public static ConservationGameData WithPushInfoScreenState(
        this ConservationGameData state,
        InfoState infoState)
    {
        return state with
        {
            InteractionData = state.InteractionData with
            {
                InfoState = [.. state.InteractionData.InfoState, infoState]
            }
        };
    }

    public static ConservationGameData WithPopInfoScreenState(
        this ConservationGameData state,
        InfoState infoState)
    {
        if (state.InteractionData.InfoState.Last() != infoState)
        {
            Console.Error.WriteLine("CANNOT POP INFO SCREEN STATE WHEN IT MISMATCHES");
            return state;
        }

        return state with
        {
            InteractionData = state.InteractionData with
            {
                InfoState = [.. state.InteractionData.InfoState.Take(state.InteractionData.InfoState.Count - 1)]
            }
        };
    }

    public static ConservationGameData WithResetRegionState(
        this ConservationGameData state)
    {
        return state with
        {
            ActiveRegion = null,
            InteractionData = state.InteractionData with
            {
                DefaultScreenData = state.InteractionData.DefaultScreenData with
                {
                    SelectedRegion = null
                },
                RegionScreenData = state.InteractionData.RegionScreenData with
                {
                    RegionId = null,
                    SelectedKakapoId = null
                }
            }
        };
    }

    public static ConservationGameData WithActiveRegion(
        this ConservationGameData state,
        int regionId)
    {
        return state with
        {
            ActiveRegion = state.Regions.First(_ => _.Id == regionId),
            InteractionData = state.InteractionData with
            {
                MainScreenState = MainScreenState.Region,
                RegionScreenData = new RegionScreenData(regionId, null)
            }
        };
    }

    public static ConservationGameData WithSetSelectedRegion(
        this ConservationGameData state,
        int regionId)
    {
        return state with
        {
            InteractionData = state.InteractionData with
            {
                DefaultScreenData = state.InteractionData.DefaultScreenData with
                {
                    SelectedRegion = regionId
                }
            }
        };
    }

    public static ConservationGameData WithScreenPanelState(
        this ConservationGameData state,
        ScreenPanelState panelState)
    {
        return state with
        {
            InteractionData = state.InteractionData with
            {
                PanelState = panelState
            }
        };
    }

    public static ConservationGameData WithActiveKakapo(
        this ConservationGameData state,
        int kakapoId)
    {
        return state with
        {
            InteractionData = state.InteractionData with
            {
                RegionScreenData = state.InteractionData.RegionScreenData with
                {
                    SelectedKakapoId = kakapoId
                }
            }
        };
    }
}
