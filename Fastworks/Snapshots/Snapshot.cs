using System;

namespace Fastworks.Snapshots
{
    [Serializable]
    public abstract class Snapshot : ISnapshot
    {
        public long Version { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid AggregateRootId { get; set; }

        public Snapshot() { }
    }
}
