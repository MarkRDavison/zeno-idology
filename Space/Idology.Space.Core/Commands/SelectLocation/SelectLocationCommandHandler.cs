namespace Idology.Space.Core.Commands.SelectLocation;

public sealed class SelectLocationCommandHandler : ISpaceCommandHandler<SelectLocationCommand>
{
    public void HandleCommand(SelectLocationCommand command)
    {
        Console.WriteLine("Selected tile {0},{1}", command.X, command.Y);
    }
}
