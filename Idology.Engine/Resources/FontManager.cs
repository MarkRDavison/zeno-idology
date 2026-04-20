namespace Idology.Engine.Resources;

public sealed class FontManager : IFontManager
{
    private readonly Dictionary<string, Font> _fonts;
    private bool disposedValue;

    public FontManager()
    {
        _fonts = new Dictionary<string, Font>();
    }

    public void LoadFont(string name, string path)
    {
        // TODO: Investigate here how to make this create more clear fonts... SDF?
        var font = Raylib.LoadFontEx(path, 96, [], 1000);
        if (_fonts.ContainsKey(name))
        {
            Raylib.UnloadFont(_fonts[name]);
            _fonts[name] = font;
        }
        else
        {
            _fonts.Add(name, font);
        }
    }

    public Font GetFont(string name)
    {
        if (_fonts.TryGetValue(name, out var font))
        {
            return font;
        }

        throw new InvalidOperationException($"Cannot find font with name '{name}'");
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                //foreach (var (name, font) in _fonts)
                //{
                //    Raylib.UnloadFont(font);
                //}
                _fonts.Clear();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
