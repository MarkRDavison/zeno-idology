namespace Idology.Core;

public static class ResourceConstants
{
    public const string MedievalSpriteSheet = nameof(MedievalSpriteSheet);
    public const string UiSpriteSheet = nameof(UiSpriteSheet);
    public const string CustomSpriteSheet = nameof(CustomSpriteSheet);
}

public static class MedievalSpriteNames
{
    public const string CastleTop = "medievalStructure_02.png";
    public const string CastleBottom = "medievalStructure_06.png";
    public const string House1 = "medievalStructure_18.png";
    public const string FarmSmallEmpty = "medievalTile_53.png";
    public const string FarmSmallFull = "medievalTile_54.png";
    public const string FarmLargeEmpty = "medievalTile_55.png";
    public const string FarmLargeFull = "medievalTile_56.png";
    public const string Grass = "medievalTile_57.png";
}
public static class CustomSpriteNames
{
    public const string BerryBush = "BerryBush.png";
    public const string BerryBushFull = "BerryBushFull.png";
    public const string BasicTent = "BasicTent.png";
    public const string BasicField = "BasicField.png";
    public const string Bakery = "Bakery.png";
}

public static class GameConstants
{
    public const int TileSize = 64;
    public const int ChunkSize = 16;
}