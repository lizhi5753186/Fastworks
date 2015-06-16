using System;

namespace Fastworks.Snapshots
{
    public interface ISnapshot
    {
        DateTime Timestamp { get; set; }

        Guid AggregateRootId { get; set; }

        long Version { get; set; }
    }
}
