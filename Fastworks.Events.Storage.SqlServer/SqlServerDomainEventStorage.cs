using Fastworks.Storage;
using Fastworks.Storage.SqlServer;

namespace Fastworks.Events.Storage.SqlServer
{
    public class SqlServerDomainEventStorage : RdbmsDomainEventStorage<SqlServerStorage>
    {
        public SqlServerDomainEventStorage(string connectionString, IStorageMappingResolver mappingResolver)
            : base(connectionString, mappingResolver)
        {
 
        }
    }
}
