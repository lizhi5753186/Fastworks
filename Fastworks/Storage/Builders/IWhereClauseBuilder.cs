using System;
using System.Linq.Expressions;

namespace Fastworks.Storage.Builders
{
    public interface IWhereClauseBuilder<T>
        where T : class, new()
    {
        /// <summary>
        /// Builds the WHERE clause from the given expression object.
        /// </summary>
        /// <param name="expression">The expression object.</param>
        /// <returns>The <c>Apworks.Storage.Builders.WhereClauseBuildResult</c> instance
        /// which contains the build result.</returns>
        WhereClauseBuildResult BuildWhereClause(Expression<Func<T, bool>> expression);
    }
}
