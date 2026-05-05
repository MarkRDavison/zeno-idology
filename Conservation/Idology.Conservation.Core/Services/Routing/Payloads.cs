namespace Idology.Conservation.Core.Services.Routing;

public sealed record KakapoSummaryInfoPanelPayload(int KakapoId);
public sealed record RegionInfoPanelPayload(int RegionId);
public sealed record PopInfoPanelPayload(InfoState InfoState);
public sealed record PushInfoPanelPayload(InfoState InfoState, object? Payload);

public sealed record OpenScreenPanelPayload(ScreenPanelState PanelState, object? Payload);
public sealed record CloseScreenPanelPayload(ScreenPanelState PanelState);
public sealed record SetMainScreenPanelPayload(MainScreenState MainScreenState, object? Payload);