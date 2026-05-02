namespace Idology.Conservation.Core.Simulation.Region;

static class KakapoDistribution
{
    public static bool SpreadOut(
        List<KakapoSimulationData> entities,
        HashSet<Vector2> validCells,
        int iterations,
        Random rng,
        int width,
        int height,
        int max,
        float wanderChance,
        bool debug = false)
    {
        // TODO: Can this handle multiple kakapo on one tile?
        for (int it = 0; it < iterations; it++)
        {
            var occupationScore = new Dictionary<Vector2, int>();

            foreach (var startKakapo in entities)
            {
                var distanceMap = new Dictionary<Vector2, int> { { startKakapo.CurrentLocation, 0 } };
                var queueForDistance = new Queue<Vector2>([startKakapo.CurrentLocation]);

                while (queueForDistance.Count > 0)
                {
                    var current = queueForDistance.Dequeue();
                    var currentDistance = distanceMap[current];

                    var neighbors = Neighbours(current, rng);
                    foreach (var neighbor in neighbors)
                    {
                        if (validCells.Contains(neighbor) && (!distanceMap.ContainsKey(neighbor) || currentDistance + 1 < distanceMap[neighbor]))
                        {
                            distanceMap[neighbor] = currentDistance + 1;
                            queueForDistance.Enqueue(neighbor);
                        }
                    }
                }

                foreach (var kvp in distanceMap)
                {
                    var location = kvp.Key;
                    var distance = kvp.Value;

                    var score = Math.Max(0, max - distance);

                    if (occupationScore.ContainsKey(location))
                    {
                        occupationScore[location] += score;
                    }
                    else
                    {
                        occupationScore[location] = score;
                    }
                }
            }

            if (debug)
            {
                DumpOccupationScore(width, height, occupationScore);
            }

            var kakapoMoved = false;

            for (int i = 0; i < entities.Count; ++i)
            {
                var k = entities[i];

                if (!occupationScore.TryGetValue(k.CurrentLocation, out var occupancy))
                {
                    continue;
                }

                var neighbours = Neighbours(k.CurrentLocation, rng)
                    .Select(_ => (_, occupationScore.TryGetValue(_, out int value) ? value : -1))
                    .Where(_ => _.Item2 >= 0)
                    .OrderBy(_ => _.Item2)
                    .ToList();

                if (!neighbours.Any())
                {
                    continue;
                }

                if (occupancy > max)
                {
                    // TODO: Move to the one farthest from the worst...

                    var lowestTargets = neighbours.Where(_ => _.Item2 == neighbours.First().Item2).ToList();

                    var movingTo = lowestTargets[Random.Shared.Next(lowestTargets.Count)];

                    entities[i] = k with { CurrentLocation = movingTo._ };
                    kakapoMoved = true;
                }
                else if ((float)(Random.Shared.Next(0, 100) / 100.0f) < wanderChance)
                {
                    const int WanderVariance = 3;
                    var lowestTargets = neighbours.Where(_ => _.Item2 <= neighbours.First().Item2 + WanderVariance).ToList();

                    var movingTo = lowestTargets[Random.Shared.Next(lowestTargets.Count)];

                    entities[i] = k with { CurrentLocation = movingTo._ };
                    kakapoMoved = true;
                }
            }

            if (!kakapoMoved)
            {
                return false;
            }
        }

        return true;
    }

    private static void DumpOccupationScore(int width, int height, Dictionary<Vector2, int> occupationScore)
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (occupationScore.TryGetValue(new Vector2(x, y), out var score))
                {
                    if (score == 0)
                    {
                        Console.Write("  . ");
                    }
                    else if (score < 10)
                    {
                        Console.Write("  {0} ", score);
                    }
                    else
                    {
                        Console.Write(" {0} ", score);
                    }
                }
                else
                {
                    Console.Write("  . ");
                }
            }

            Console.WriteLine();
        }
    }

    static List<Vector2> Neighbours(Vector2 p, Random rng)
    {
        var n = new List<Vector2>
        {
            p + new Vector2( 1,  0),
            p + new Vector2(-1,  0),
            p + new Vector2( 0,  1),
            p + new Vector2( 0, -1),
            p + new Vector2( 1,  1),
            p + new Vector2( 1, -1),
            p + new Vector2(-1,  1),
            p + new Vector2(-1, -1),
        };

        Shuffle(n, rng);

        return n;
    }

    static void Shuffle<T>(List<T> list, Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}