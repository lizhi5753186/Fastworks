using Fastworks.Storage.Builders;
using Fastworks.Storage.SqlServer.Builders;
using System.Data.Common;
using System.Data.SqlClient;

namespace Fastworks.Storage.SqlServer
{
    /// <summary>
    /// Represents SQL Server storage
    /// </summary>
    public class SqlServerStorage :RdbmsStorage
    {
        public SqlServerStorage(string connectionString, IStorageMappingResolver mappingresolver) 
            : base(connectionString, mappingresolver) 
        { 
        }

        #region Override RdbmsStorage Members
        protected override DbConnection CreateDatabaseConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        protected override DbParameter CreateParameter()
        {
            return new SqlParameter();
        }

        protected override DbCommand CreateCommand(string sql, DbConnection connection)
        {
            SqlCommand command = new SqlCommand(sql, connection as SqlConnection);
            return command;
        }

        protected override WhereClauseBuilder<T> CreateWhereClauseBuilder<T>()
        {
            return new SqlServerWhereClauseBuilder<T>(MappingResolver);
        }
        #endregion 


        public override bool DistributedTransactionSupported
        {
            get { return true; }
        }
    }
}
