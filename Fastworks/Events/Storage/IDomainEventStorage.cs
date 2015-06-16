using System;
using System.Collections.Generic;

namespace Fastworks.Events.Storage
{
    public interface IDomainEventStorage : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Saves the specified domain event to the event storage.
        /// </summary>
        /// <param name="domainEvent">The domain event to be saved.</param>
        void SaveEvent(IDomainEvent domainEvent);
        /// <summary>
        /// Loads all the domain events for the specific aggregate root from the storage.
        /// </summary>
        /// <param name="aggregateRootType">The type of the aggregate root.</param>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>A list of domain events for the specific aggregate root.</returns>
        IEnumerable<IDomainEvent> LoadEvents(Type aggregateRootType, Guid id);
        /// <summary>
        /// Loads all the domain events for the specific aggregate root from the storage.
        /// </summary>
        /// <param name="aggregateRootType">The type of the aggregate root.</param>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <param name="version">The version number.</param>
        /// <returns>A list of domain events for the specific aggregate root which occur just after
        /// the given version number.</returns>
        IEnumerable<IDomainEvent> LoadEvents(Type aggregateRootType, Guid id, long version);
    }
}
