using System.Linq.Expressions;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.OCP.Good;
internal class NotEqualStrategy<TEntity, TProperty>() : IFilterOperatorStrategy<TEntity, TProperty>
{
    public Expression<Func<TEntity, bool>> GetFilterExpression(TProperty filter, Expression<Func<TEntity, TProperty>> filteredPropertyExpr)
    {
        return IFilterOperatorStrategy<TEntity, TProperty>.CreateFilterExpression(filter, filteredPropertyExpr, Expression.NotEqual);
    }
}