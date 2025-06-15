namespace Idology.Space.Core;

public static class SpaceConstants
{
    private static JsonSerializerOptions? _defaultOptions;
    public const int TileSize = 32;
    public static JsonSerializerOptions DefaultJsonSerializerOptions
    {
        get
        {
            if (_defaultOptions is null)
            {
                _defaultOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                _defaultOptions.Converters.Add(new Vector2JsonConverter());
                _defaultOptions.Converters.Add(new RaylibColorJsonConverter());
            }

            return _defaultOptions;
        }
    }
}
