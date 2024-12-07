using System.Linq.Expressions;
using System.Numerics;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.OCP.Bad;

/*
 * Why does this class violate the Open-Closed Principle?
 * 
 * The DynamicFilterQuery class does very different things based on an enum value.
 * This means it is responsible for interpreting the behavior of each filter operator,
 * validating input data based on the operator, and constructing the corresponding 
 * filter expressions. Finally, it is also responsible for filtering entities 
 * using the generated expression.
 * 
 * The design of this class is hard to maintain and extend because it violates the OCP principle.
 * For example, adding a new FilterOperator would require changes in multiple places:
 * - The validation logic in the Create method
 * - The expression construction logic within the switch cases in the Create method
 * - Any additional places where operators might need to be handled in the future
 * 
 * This class is therefore close for extension and open for modification which is the exact
 * opposite of what the Open-Close Principle suggests.
 * 
 * This class is not closed for modification because its core logic needs to be changed 
 * whenever a new FilterOperator is introduced. To adhere to the Open-Closed Principle, 
 * the class should allow extension without requiring modifications.
 * 
 * Ideally, all of these operators should be implemented as different strategy classes.
 * Each strategy class would encapsulate the behavior of a single FilterOperator, including
 * validation, expression construction, and any operator-specific logic. The main 
 * DynamicFilterQuery class would then delegate the handling of FilterOperators to 
 * these strategy classes. This approach would make the class open for extension 
 * (by adding new strategy classes for new operators) without requiring changes to 
 * the existing logic, adhering to the Open-Closed Principle.
 */

//Also allows passing custom filters through the constructor
internal class DynamicFilterQuery<TEntity, TProperty>(Expression<Func<TEntity, bool>> filterExpression)
{
    internal static DynamicFilterQuery<TEntity, TProperty> Create(FilterOperator filterOperator,
        TProperty filter, Expression<Func<TEntity, TProperty>> filteredPropertyExpr)
    {
        if(!Enum.IsDefined(typeof(FilterOperator), filterOperator))
        {
            throw new ArgumentException("Invalid operator", nameof(filterOperator));
        }

        if (filteredPropertyExpr is null)
        {
            throw new ArgumentNullException(nameof(filteredPropertyExpr), "Filtered property expression can't be null");
        }
        
        bool isEqualityOperator = filterOperator is (FilterOperator.Equal or FilterOperator.NotEqual);

        if (filter is null && !isEqualityOperator)
        {
            throw new ArgumentNullException(nameof(filter), "Filter can't be null except for equality checks");
        }

        bool isStringOperator = filterOperator is (FilterOperator.Contains or FilterOperator.StartsWith or FilterOperator.EndsWith);
        Type numberType = typeof(INumber<>).MakeGenericType(typeof(TProperty));
        bool isNumberOrDate = typeof(TProperty).IsAssignableTo(numberType) || typeof(TProperty) == typeof(DateTime);

        if (!isNumberOrDate && (!isStringOperator && !isEqualityOperator))
        {
            throw new ArgumentException("Only number and DateTime types are allowed in comparisons other than equality checks");
        }

        bool isString = typeof(TProperty) == typeof(string);

        if (!isString && isStringOperator)
        {
            throw new ArgumentException($"Only strings are allowed with {nameof(FilterOperator.Contains)}, {nameof(FilterOperator.StartsWith)}, " +
                $"and {nameof(FilterOperator.EndsWith)} operators");
        }

        //We can get rid of these switch-cases by making a static dictionary and pulling the values from there
        //But this is just a demonstration
        ConstantExpression filterConstantExpr = filterOperator switch
        {
            FilterOperator.Contains => Expression.Constant($"%{filter}%", typeof(string)),
            FilterOperator.StartsWith => Expression.Constant($"%{filter}%", typeof(string)),
            FilterOperator.EndsWith => Expression.Constant($"%{filter}%", typeof(string)),
            _ => Expression.Constant(filter, typeof(TProperty)),
        };

        Expression filterBodyExpr = filterOperator switch
        {
            FilterOperator.Equal => filterBodyExpr = Expression.Equal(filteredPropertyExpr, filterConstantExpr),
            FilterOperator.NotEqual => filterBodyExpr = Expression.NotEqual(filteredPropertyExpr, filterConstantExpr),
            FilterOperator.LessThan => filterBodyExpr = Expression.LessThan(filteredPropertyExpr, filterConstantExpr),
            FilterOperator.LessThanOrEqual => filterBodyExpr = Expression.LessThanOrEqual(filteredPropertyExpr, filterConstantExpr),
            FilterOperator.GreaterThan => filterBodyExpr = Expression.GreaterThan(filteredPropertyExpr, filterConstantExpr),
            FilterOperator.GreaterThanOrEqual => filterBodyExpr = Expression.GreaterThanOrEqual(filteredPropertyExpr, filterConstantExpr),
            FilterOperator.Contains => ExpressionUtils.LikeExpression(filteredPropertyExpr, filterConstantExpr),
            FilterOperator.StartsWith => ExpressionUtils.LikeExpression(filteredPropertyExpr, filterConstantExpr),
            FilterOperator.EndsWith => ExpressionUtils.LikeExpression(filteredPropertyExpr, filterConstantExpr),
            _ => throw new ArgumentOutOfRangeException(nameof(FilterOperator), filterOperator, "Unsupported or new filter operator!"),
        };

        ParameterExpression entityParamExpr = Expression.Parameter(typeof(TEntity), nameof(TEntity));
        Expression<Func<TEntity, bool>> filterExpression = Expression.Lambda<Func<TEntity, bool>>(filterBodyExpr, entityParamExpr);
        return new DynamicFilterQuery<TEntity, TProperty>(filterExpression);
    }

    internal IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities)
    {
        return entities.Where(filterExpression);
    }
}