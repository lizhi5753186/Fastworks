using Fastworks.Events;
using Fastworks.Snapshots;
using System.Collections.Generic;

namespace Fastworks
{
    public interface ISourcedAggregateRoot : IAggregateRoot, ISnapshotOrignator
    {
        void BuildFromHistory(IEnumerable<IDomainEvent> historicalEvents);

        // Gets all the uncommitted events.
        IEnumerable<IDomainEvent> UncommittedEvents { get; }

        long Version { get; }
    }
}
