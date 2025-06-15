using System.Numerics;

namespace Idology.Space.Client.Helpers;

public static class LevelCreationHelper
{
    public static LevelData CreateTestLevel()
    {
        const int LevelSize = 32;

        var level = new LevelData
        {
            Width = LevelSize,
            Height = LevelSize,
            Tiles =
            [..
                Enumerable
                    .Range(0, LevelSize)
                    .Select<int, List<TileData>>(_ =>
                    [..
                        Enumerable
                            .Range(0, LevelSize)
                            .Select(_ => new TileData
                            {
                                IsEmpty = true
                            })
                    ])
            ]
        };

        for (int y = 4; y <= 16; ++y)
        {
            for (int x = 4; x <= 24; ++x)
            {
                if (y == 4 || y == 16 || x == 24 || x == 4)
                {
                    level.Tiles[y][x].IsEmpty = false;
                }
            }
        }

        level.Creatures.Add(new Creature
        {
            Position = new Vector2(1.5f, 1.5f)
        });
        level.Creatures.Add(new Creature
        {
            Position = new Vector2(28.5f, 3.5f)
        });
        level.Creatures.Add(new Creature
        {
            Position = new Vector2(14.5f, 23.5f)
        });

        return level;
    }
}
