using Fastworks.Storage.Builders;

namespace Fastworks.Storage.SqlServer.Builders
{
    public sealed class SqlServerWhereClauseBuilder<TDataObject> : WhereClauseBuilder<TDataObject>
        where TDataObject : class, new()
    {

        public SqlServerWhereClauseBuilder(IStorageMappingResolver mappingResolver)
            :base(mappingResolver)
        { }

        #region WhereClauseBuilder Members
        protected override char ParameterChar
        {
            get { return '@'; }
        }
        #endregion 
    }
}
