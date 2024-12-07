using System.Linq.Expressions;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.OCP.Good;
internal class EndsWithStrategy<TEntity, TProperty>() : IFilterOperatorStrategy<TEntity, TProperty>
{
    public Expression<Func<TEntity, bool>> GetFilterExpression(TProperty filter, Expression<Func<TEntity, TProperty>> filteredPropertyExpr)
    {
        IFilterOperatorStrategy<TEntity, TProperty>.ValidateForString();
        IFilterOperatorStrategy<TEntity, TProperty>.ValidateForNull(filter);
        return IFilterOperatorStrategy<TEntity, TProperty>.CreateFilterExpression($"%{filter}",
            filteredPropertyExpr, ExpressionUtils.LikeExpression);
    }
}