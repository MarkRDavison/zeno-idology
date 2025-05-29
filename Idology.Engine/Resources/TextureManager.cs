namespace Idology.Engine.Resources;

public sealed class TextureManager : ITextureManager
{
    private readonly Dictionary<string, Texture2D> _textures;
    private bool disposedValue;

    public TextureManager()
    {
        _textures = new Dictionary<string, Texture2D>();
    }

    public void LoadTexture(string name, string path)
    {
        var texture = Raylib.LoadTexture(path);
        if (_textures.ContainsKey(name))
        {
            Raylib.UnloadTexture(_textures[name]);
            _textures[name] = texture;
        }
        else
        {
            _textures.Add(name, texture);
        }
    }

    public Texture2D GetTexture(string name)
    {
        if (_textures.TryGetValue(name, out var texture))
        {
            return texture;
        }

        throw new InvalidOperationException($"Cannot find texture with name '{name}'");
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                foreach (var (name, texture) in _textures)
                {
                    Raylib.UnloadTexture(texture);
                }
                _textures.Clear();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
