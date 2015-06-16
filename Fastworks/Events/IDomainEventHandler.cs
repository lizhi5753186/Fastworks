
namespace Fastworks.Events
{
    public interface IDomainEventHandler<in TDomainEvent> : IEventHandler<TDomainEvent>
       where TDomainEvent : class, IDomainEvent
    {

    }
}
