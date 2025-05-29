namespace Idology.Outpost.Core.Commands;

public sealed class DummyCommand : IGameCommand
{
    public DummyCommand(string name)
    {
        Name = name;
    }
    public string Name { get; }
}
