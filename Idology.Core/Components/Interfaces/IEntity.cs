namespace Idology.Core.Components.Interfaces;

public interface IEntity
{
    public Guid Id { get; set; }
    public Guid PrototypeId { get; set; }
}
