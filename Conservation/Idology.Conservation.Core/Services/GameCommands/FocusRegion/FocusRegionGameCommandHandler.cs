namespace Idology.Conservation.Core.Services.GameCommands.FocusRegion;

internal sealed class FocusRegionGameCommandHandler : IGameCommandHandler<FocusRegionGameCommand>
{
    private readonly ConservationGameData _gameData;
    private readonly ConservationGameCamera _camera;
    private readonly IInputManager _inputManager;

    public FocusRegionGameCommandHandler(
        ConservationGameData gameData,
        ConservationGameCamera camera,
        IInputManager inputManager)
    {
        _gameData = gameData;
        _camera = camera;
        _inputManager = inputManager;
    }

    public bool HandleCommand(FocusRegionGameCommand command)
    {
        var region = _gameData.Regions.First(_ => _.Id == command.RegionId);

        var baseZoomRegionOffset = region.RegionOffset * Constants.TileSize;

        if (_gameData.InteractionData.ScreenState is ScreenState.Default)
        {
            var infoPanelOpen = _gameData.InteractionData.InfoState is not InfoState.Hidden;

            var size = _inputManager.GetScreenSize();

            // TODO: Some service that knows this...
            var availableWidth = infoPanelOpen ? (size.X - InfoContextPanelWidget.Width - InfoContextPanelWidget.Padding) : size.X;
            var availableHeight = size.Y - TopBarWidget.Height;

            var regionWidth = region.Width * Constants.TileSize;
            var regionHeight = region.Height * Constants.TileSize;

            var regionToAvailableWidth = availableWidth / regionWidth;
            var regionToAvailableHeight = availableHeight / regionHeight;

            Console.WriteLine("=========================================");
            Console.WriteLine("Available: {0}x{1}", availableWidth, availableHeight);
            Console.WriteLine("Region: {0}x{1}", regionWidth, regionHeight);
            Console.WriteLine("Ratio: {0}x{1}", regionToAvailableWidth, regionToAvailableHeight);

            var regionCenteringOffset = new Vector2();

            if (regionToAvailableHeight == regionToAvailableWidth)
            {
                Console.WriteLine("Doesnt matter, both dimensions equal");
            }
            else if (regionToAvailableHeight < regionToAvailableWidth)
            {
                Console.WriteLine("Height is the constraint, so the region is taller than it is wide, or the available space is wider than it is tall");

                Console.WriteLine("So the height ratio of {0} compared to zoom of {1} means we need to adjust zoom", regionToAvailableHeight, _camera.Zoom);

                Console.WriteLine("We also need to offset the x position so the region is centered");

                _camera.Zoom = regionToAvailableHeight;

                regionCenteringOffset.X = ((availableWidth - regionWidth) / 2.0f) * _camera.Zoom;
            }
            else
            {
                Console.WriteLine("Width is the constraint, so the region is wider than it is tall, or the available space is taller than it is wide");

                Console.WriteLine("So the width ratio of {0} compared to zoom of {1} means we need to adjust zoom", regionToAvailableWidth, _camera.Zoom);

                Console.WriteLine("We also need to offset the y position so the region is centered");

                _camera.Zoom = regionToAvailableWidth;

                // Truly do not understand why the y does not need to be adjusted by the zoom...
                regionCenteringOffset.Y = ((availableHeight - regionHeight) / 2.0f) * 1.0f;// _camera.Zoom;
            }

            Console.WriteLine("At this zoom the region will fill {0},{1} pixels", regionWidth * _camera.Zoom, regionHeight * _camera.Zoom);

            Console.WriteLine("RegionCenteringOffset: {0},{1}", regionCenteringOffset.X, regionCenteringOffset.Y);

            _camera.Target =
                new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2) / _camera.Zoom
                +
                new Vector2(baseZoomRegionOffset.X, baseZoomRegionOffset.Y)
                +
                regionCenteringOffset;

            //_camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2) - new Vector2(baseZoomRegionOffset.X, baseZoomRegionOffset.Y) * _camera.Zoom;
            return true;
        }
        else if (_gameData.InteractionData.ScreenState is ScreenState.Region)
        {
            //_camera.Offset = new Vector2(0, 0);
            return true;
        }

        return false;
    }
}
