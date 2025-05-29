namespace Idology.Core.Systems;

public sealed class WorkplaceProcessSystem : WorldSystem
{
    public override void Update(World world, float delta)
    {
        var workplaces = world.GetWithAll<WorkplaceComponent>();

        foreach (var workplace in workplaces)
        {
            var wc = workplace.Get<WorkplaceComponent>();
            var bc = workplace.Get<BuildingComponent>();

            // TODO: If partially populated give partial output
            if (wc.Job.Any() && wc.Job.All(_ => _.Value.Available == 0))
            {
                // TODO: Need to have the inputs to run production, so if starting grab them, store in 
                // workplace inventory
                // then we can start processing them
                if (wc.Production.Inputs.Any())
                {
                }

                wc.ProductionTime += delta;
                if (wc.ProductionTime >= wc.Production.Time)
                {
                    wc.ProductionTime -= wc.Production.Time;
                    foreach (var o in wc.Production.Outputs)
                    {
                        var amount = Random.Shared.Next(o.Value.Min, o.Value.Max + 1);
                        Console.WriteLine(" - Created {0} {1}", amount, o.Key);
                    }
                }
            }
        }
    }
}
