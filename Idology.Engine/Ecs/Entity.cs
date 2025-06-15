namespace Idology.Engine.Ecs;

public class Entity
{
    private List<object> _components = [];

    public Guid Id { get; set; } = Guid.NewGuid();

    public void Add<TC>(TC component)
    {
        _components.Add(component!);
    }

    public bool Has<TC>()
    {
        return _components.Any(_ => _ is TC);
    }

    public bool Has<TC0, TC1>()
    {
        return Has<TC0>() && Has<TC1>();
    }

    public bool Has<TC0, TC1, TC2>()
    {
        return Has<TC0>() && Has<TC1>() && Has<TC2>();
    }

    public bool Has<TC0, TC1, TC2, TC3>()
    {
        return Has<TC0>() && Has<TC1>() && Has<TC2>() && Has<TC3>();
    }

    public TC Get<TC>() => (TC)_components.First(_ => _ is TC);

    public void Remove<TC>() => _components.Remove(Get<TC>()!);
}
