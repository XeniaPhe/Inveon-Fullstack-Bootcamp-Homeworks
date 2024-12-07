using System.Linq.Expressions;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.OCP.Good;

//Can still pass custom filters through the constructor
internal class DynamicFilterQuery<TEntity, TProperty>(Expression<Func<TEntity, bool>> filterExpression)
{
    //All the expression creation and validation logic has been moved to the strategy classes
    //Now this class is open for extension but closed for modification since any logic change
    //would have to be done inside a strategy class without having to modify this class.
    internal static DynamicFilterQuery<TEntity, TProperty> Create(
        IFilterOperatorStrategy<TEntity, TProperty> filterOperatorStrategy,
        TProperty filter,
        Expression<Func<TEntity, TProperty>> filteredPropertyExpr)
    {
        Expression<Func<TEntity, bool>> filterExpression = filterOperatorStrategy.GetFilterExpression(filter, filteredPropertyExpr);
        return new DynamicFilterQuery<TEntity, TProperty>(filterExpression);
    }

    internal IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities)
    {
        return entities.Where(filterExpression);
    }
}