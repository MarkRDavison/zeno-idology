namespace Idology.Core.Systems;

public sealed class DebugUiSystem : WorldSystem
{
    public override void Update(World world, float delta)
    {
    }

    public override void UpdateNoCamera(World world, float delta)
    {
        var currentAccomodation = new Dictionary<Guid, AmountRange>();
        var currentJobs = new Dictionary<Guid, AmountRange>();

        foreach (var e in world.GetWithAll<BuildingComponent>())
        {
            var b = e.Get<BuildingComponent>();
            foreach (var (id, r) in b.Accomodation)
            {
                if (currentAccomodation.ContainsKey(id))
                {
                    currentAccomodation[id].Merge(r);
                }
                else
                {
                    currentAccomodation.Add(id, r.Clone());
                }
            }
        }

        foreach (var e in world.GetWithAll<WorkplaceComponent>())
        {
            var w = e.Get<WorkplaceComponent>();

            foreach (var (id, r) in w.Job)
            {
                if (currentJobs.ContainsKey(id))
                {
                    currentJobs[id].Merge(r);
                }
                else
                {
                    currentJobs.Add(id, r.Clone());
                }
            }
        }

        var l = world.GetWithAll<LevelComponent>().First().Get<LevelComponent>();
        int offset = 0;
        if (l.TotalAccomodation.Any())
        {
            Raylib.DrawText("Accomodation", 10, 32, 20, Color.Black);
            offset += 24;
            foreach (var (id, ar) in currentAccomodation)
            {
                Raylib.DrawText($"{StringHash.ReverseHash(id)} {ar.Current}/{ar.Max}", 16, 32 + offset, 20, Color.Black);

                offset += 24;
            }
        }
        if (l.TotalWorkers.Any())
        {
            Raylib.DrawText("Workers", 10, 32 + offset, 20, Color.Black);
            offset += 24;
            foreach (var (id, count) in l.TotalWorkers)
            {
                Raylib.DrawText($"{StringHash.ReverseHash(id)} {count}", 16, 32 + offset, 20, Color.Black);

                offset += 24;
            }
        }
        if (l.TotalJobs.Any())
        {
            Raylib.DrawText("Jobs", 10, 32 + offset, 20, Color.Black);
            offset += 24;
            foreach (var (id, range) in currentJobs)
            {
                Raylib.DrawText($"{StringHash.ReverseHash(id)} {range.Current}/{range.Max}", 16, 32 + offset, 20, Color.Black);

                offset += 24;
            }
        }
    }
}
