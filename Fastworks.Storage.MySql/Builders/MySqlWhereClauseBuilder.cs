using Fastworks.Storage.Builders;

namespace Fastworks.Storage.MySql.Builders
{
    public sealed class MySqlWhereClauseBuilder<TDataObject> : WhereClauseBuilder<TDataObject>
        where TDataObject : class, new()
    {
        public MySqlWhereClauseBuilder(IStorageMappingResolver mappingResolver)
            :base(mappingResolver)
        { }

        #region WhereClauseBuilder Members
        protected override char ParameterChar
        {
            get { return '?'; }
        }
        #endregion 
    }
}
