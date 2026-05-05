namespace Idology.Conservation.Core.Services.Camera;

internal sealed class CameraService : ICameraService
{
    private readonly IConservationStateService _gameState;
    private readonly ConservationGameCamera _camera;
    private readonly IInputManager _inputManager;

    public CameraService(
        IConservationStateService gameState,
        ConservationGameCamera camera,
        IInputManager inputManager)
    {
        _gameState = gameState;
        _camera = camera;
        _inputManager = inputManager;
    }

    public bool FocusOnRegion(int regionId)
    {
        var region = _gameState.State.Regions.First(_ => _.Id == regionId);

        var baseZoomRegionOffset = region.RegionOffset * Constants.TileSize;

        // TODO: Does this need to be re-invoked when changing screen size???
        // More realistically we need to calculate a "zoom in/out" based on the center
        // of the non top bar and non info panel area to retain what the user is looking at...
        if (_gameState.State.InteractionData.PanelState is ScreenPanelState.None)
        {
            var infoPanelOpen = _gameState.State.InteractionData.InfoState.Any();

            var size = _inputManager.GetScreenSize();

            // TODO: Some service that knows this...
            var availableWidth = infoPanelOpen ? (size.X - InfoContextPanelWidget.Width - InfoContextPanelWidget.Padding) : size.X;
            var availableHeight = size.Y - TopBarWidget.Height;

            var regionWidth = region.Width * Constants.TileSize;
            var regionHeight = region.Height * Constants.TileSize;

            var regionToAvailableWidth = availableWidth / regionWidth;
            var regionToAvailableHeight = availableHeight / regionHeight;

            var regionCenteringOffset = new Vector2();

            if (regionToAvailableHeight <= regionToAvailableWidth)
            {
                _camera.Zoom = regionToAvailableHeight;
                regionCenteringOffset.X = (availableWidth - regionWidth * regionToAvailableHeight) / 2.0f;
            }
            else
            {
                _camera.Zoom = regionToAvailableWidth;
                regionCenteringOffset.Y = (availableHeight - regionHeight * regionToAvailableWidth) / 2.0f;
            }

            _camera.Target =
                new Vector2(baseZoomRegionOffset.X, baseZoomRegionOffset.Y)
                -
                regionCenteringOffset / _camera.Zoom;

            return true;
        }

        return false;
    }
}
