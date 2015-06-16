
namespace Fastworks.Snapshots
{
    // Represents that the implemented classes are snapshot originators.
    public interface ISnapshotOrignator
    {
        // Builds the originator from the specific snapshot.
        void BuildFromSnapshot(ISnapshot snapshot);

        ISnapshot CreateSnapshot();
    }
}
