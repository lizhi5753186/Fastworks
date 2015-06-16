using Fastworks.Storage;
using Fastworks.Storage.MySql;

namespace Fastworks.Events.Storage.MySql
{
    public class MySqlDomainEventStorage : RdbmsDomainEventStorage<MySqlStorage>
    {
        public MySqlDomainEventStorage(string connectionString, IStorageMappingResolver mappingResolver)
            : base(connectionString, mappingResolver)
        {
        }
    }
}
