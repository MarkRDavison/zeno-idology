namespace Idology.Engine.Ecs;

public class World
{
    private readonly IList<Entity> _entities = [];

    public Entity Create(params object[] components)
    {
        var e = new Entity();

        foreach (var c in components)
        {
            e.Add(c);
        }

        _entities.Add(e);

        return e;
    }

    public TC0 GetFirst<TC0>() => _entities.First(_ => _.Has<TC0>()).Get<TC0>();

    public IEnumerable<Entity> GetWithAll<TC0>()
    {
        return _entities.Where(_ => _.Has<TC0>()).ToList();
    }

    public IEnumerable<Entity> GetWithAll<TC0, TC1>()
    {
        return _entities.Where(_ => _.Has<TC0, TC1>()).ToList();
    }

    public IEnumerable<Entity> GetWithAll<TC0, TC1, TC2>()
    {
        return _entities.Where(_ => _.Has<TC0, TC1, TC2>()).ToList();
    }
}
