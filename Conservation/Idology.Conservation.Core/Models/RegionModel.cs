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

    public RegionData ToRegionData()
    {
        var data = new RegionData
        {
            Width = RegionImage.Width,
            Height = RegionImage.Height
        };

        for (var y = 0; y < RegionImage.Height; ++y)
        {
            for (var x = 0; x < RegionImage.Width; ++x)
            {
                var imageColor = Raylib.GetImageColor(RegionImage, x, y);

                var tt = TileType.Unset;

                if (imageColor.Equals(new Color(0, 255, 0)))
                {
                    tt = TileType.Land;
                }
                else if (imageColor.Equals(new Color(0, 0, 255)))
                {
                    tt = TileType.Water;
                }

                data.Tiles.Add(new Tile(tt));
            }
        }

        return data;
    }
}
