using Fastworks.Storage;

namespace Fastworks.Snapshots.Providers
{
    /// <summary>
    /// Represents the snapshot providers that utilize both event storage and snapshot storage to
    /// implement their functionalities.
    /// </summary>
    public abstract class StorageBasedSnapshotProvider : SnapshotProvider
    {
        private readonly IStorage eventStorage;
        private readonly IStorage snapshotStorage;

        public IStorage EventStorage
        {
            get { return this.eventStorage; }
        }

        public IStorage SnapshotStorage
        {
            get { return this.snapshotStorage; }
        }

        public StorageBasedSnapshotProvider(IStorage eventStorage, IStorage snapshotStorage, SnapshotProviderOption option)
            : base(option)
        {
            this.eventStorage = eventStorage;
            this.snapshotStorage = snapshotStorage;
        }

        #region override Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.eventStorage.Dispose();
                this.snapshotStorage.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
