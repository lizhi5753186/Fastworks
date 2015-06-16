using System;
using System.Linq.Expressions;

namespace Fastworks.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public static Specification<T> Eval(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }

        #region ISpecification<T> Members
        public bool IsSatisfiedBy(T candidate)
        {
            return this.Expression.Compile()(candidate);
        }

        public abstract Expression<Func<T, bool>> Expression { get; }
        #endregion
    }
}
