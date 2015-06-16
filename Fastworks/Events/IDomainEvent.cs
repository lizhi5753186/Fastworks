
namespace Fastworks.Events
{
    /// <summary>
    /// Represents that the implemented classes are domain events.
    /// </summary>
    /// <remarks>Domain events are the events raised by domain model.</remarks>
    public interface IDomainEvent : IEvent
    {
        IEntity Source { get; set; }
        long Version { get; set; }
    }
}
