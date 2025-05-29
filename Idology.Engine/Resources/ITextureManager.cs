namespace Idology.Engine.Resources;

public interface ITextureManager : IDisposable
{
    void LoadTexture(string name, string path);
    Texture2D GetTexture(string name);
}
