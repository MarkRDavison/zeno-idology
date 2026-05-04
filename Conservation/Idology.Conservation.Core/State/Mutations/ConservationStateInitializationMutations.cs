namespace Idology.Conservation.Core.State.Mutations;

public static class ConservationStateInitializationMutations
{
    public static ConservationGameData CreateDefaultData()
    {
        return new ConservationGameData(
            new ConservationInteractionData(
                InfoState.Hidden,
                ScreenState.Default,
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

    public static ConservationGameData WithSetRegionData(
        this ConservationGameData state,
        IList<RegionData> regions)
    {
        return state with
        {
            Regions = [.. regions]
        };
    }

    public static ConservationGameData WithSetRegionSimulations(
        this ConservationGameData state,
        IList<RegionSimulation> regionSimulations)
    {
        return state with
        {
            RegionSimulations = [.. regionSimulations]
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
        ScreenState screenState)
    {
        return state with
        {
            InteractionData = state.InteractionData with
            {
                ScreenState = screenState
            }
        };
    }

    public static ConservationGameData WithInfoScreenState(
        this ConservationGameData state,
        InfoState infoState)
    {
        return state with
        {
            InteractionData = state.InteractionData with
            {
                InfoState = infoState
            }
        };
    }

    public static ConservationGameData WithResetInfoScreenStateOnHidden(
        this ConservationGameData state,
        InfoState infoState)
    {
        if (infoState is InfoState.Hidden)
        {
            return state with
            {
                InteractionData = state.InteractionData with
                {
                    RegionScreenData = state.InteractionData.RegionScreenData with
                    {
                        RegionId = -1,
                        SelectedKakapoId = null
                    }
                }
            };
        }

        return state;
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
}
