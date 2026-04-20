namespace Idology.Conservation.Core.Models;

public sealed class RegionModel
{
    private RegionModel()
    {
    }

    public static RegionModel Create(string name)
    {
        var regionPath = string.Format("Assets/Regions/{0}", name);

        var regionDataText = File.ReadAllText(regionPath + ".json");

        if (JsonSerializer.Deserialize<RegionModelData>(regionDataText) is { } regionData)
        {
            return new RegionModel
            {
                RegionImage = Raylib.LoadImage(regionPath + ".png"),
                RegionModelData = regionData
            };
        }

        throw new InvalidDataException("Invalid region");
    }

    public required Image RegionImage { get; init; }
    public required RegionModelData RegionModelData { get; init; }
}
