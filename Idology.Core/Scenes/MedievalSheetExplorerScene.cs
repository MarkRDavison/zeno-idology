
namespace Idology.Core.Scenes;

public sealed class MedievalSheetExplorerScene : SpriteSheetExplorerScene
{
    public MedievalSheetExplorerScene(ISpriteSheetManager spriteSheetManager, IFontManager fontManager) : base(spriteSheetManager, fontManager)
    {
    }

    protected override string Name => ResourceConstants.MedievalSpriteSheet;
}
