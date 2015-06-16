using Fastworks.Storage.Builders;
using Fastworks.Storage.MySql.Builders;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Fastworks.Storage.MySql
{
    public class MySqlStorage : RdbmsStorage
    {
        public MySqlStorage(string connectionString, IStorageMappingResolver mappingResolver)
            : base(connectionString, mappingResolver)
        { }

        #region Override RdbmsStorage Members
        protected override DbConnection CreateDatabaseConnection()
        {
            return new MySqlConnection(this.ConnectionString);
        }

        protected override DbParameter CreateParameter()
        {
            return new MySqlParameter();
        }

        protected override DbCommand CreateCommand(string sql, DbConnection connection)
        {
            return new MySqlCommand(sql, connection as MySqlConnection);
        }

        protected override WhereClauseBuilder<T> CreateWhereClauseBuilder<T>()
        {
            return new MySqlWhereClauseBuilder<T>(MappingResolver);
        }

        #endregion 


        public override bool DistributedTransactionSupported
        {
            get { return false; }
        }
    }
}
