namespace Idology.Conservation.Core.Simulation.Region;

static class IslandRelaxation
{
    public static void Relax(
        List<KakapoSimulationData> entities,
        HashSet<Vector2> validCells,
        int iterations,
        Random rng)
    {

        for (int it = 0; it < iterations; it++)
        {
            var occupied = new HashSet<Vector2>();
            var occupationScore = new Dictionary<Vector2, int>();

            foreach (var e in entities)
            {
                occupied.Add(e.CurrentLocation);
            }
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

    static void Shuffle<T>(IList<T> list, Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}