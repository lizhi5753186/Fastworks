
namespace Fastworks
{
    public interface IUnitOfWork
    {
        bool DistributedTransactionSupported { get; }
        bool Committed { get; }
        void Commit();
        void Rollback();
    }
}
