using System.Linq.Expressions;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.OCP.Good;
internal class LessThanStrategy<TEntity, TProperty>() : IFilterOperatorStrategy<TEntity, TProperty>
{
    public Expression<Func<TEntity, bool>> GetFilterExpression(TProperty filter, Expression<Func<TEntity, TProperty>> filteredPropertyExpr)
    {
        IFilterOperatorStrategy<TEntity, TProperty>.ValidateForNumberAndDateTime();
        IFilterOperatorStrategy<TEntity, TProperty>.ValidateForNull(filter);
        return IFilterOperatorStrategy<TEntity, TProperty>.CreateFilterExpression(filter, filteredPropertyExpr, Expression.LessThan);
    }
}