namespace Idology.Conservation.Core.Services.Routing;

public sealed record RegionInfoPanelPayload(int RegionId);

public sealed record PushInfoPanelPayload(InfoState InfoState, object? Payload);