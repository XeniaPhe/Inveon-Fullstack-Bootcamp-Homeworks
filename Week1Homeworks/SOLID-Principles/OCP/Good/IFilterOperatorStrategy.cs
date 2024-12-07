using System.Linq.Expressions;
using System.Numerics;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.OCP.Good;

//This could also be done in an abstract class without requiring static methods while still protecting the protected access modifiers
//I opted for this approach because interfaces are more flexible
internal interface IFilterOperatorStrategy<TEntity, TProperty>
{
    Expression<Func<TEntity, bool>> GetFilterExpression(TProperty filter, Expression<Func<TEntity, TProperty>> filteredPropertyExpr);

    protected static Expression<Func<TEntity, bool>> CreateFilterExpression(
        object? filter,
        Expression<Func<TEntity, TProperty>> filteredPropertyExpr,
        Func<Expression, Expression, Expression> bodyExprFunc)
    {
        if (filteredPropertyExpr is null)
        {
            throw new ArgumentNullException(nameof(filteredPropertyExpr), "Filtered property expression can't be null");
        }

        ConstantExpression filterConstantExpr = Expression.Constant(filter, typeof(TProperty));
        Expression bodyExpr = bodyExprFunc(filteredPropertyExpr, filterConstantExpr);
        ParameterExpression paramExpr = Expression.Parameter(typeof(TEntity), nameof(TEntity));
        return Expression.Lambda<Func<TEntity, bool>>(bodyExpr, paramExpr);
    }

    protected static void ValidateForNull(TProperty filter)
    {
        if (filter is null)
        {
            throw new ArgumentNullException("This operator does not accept null filter constants");
        }
    }

    protected static void ValidateForString()
    {
        if (typeof(TProperty) != typeof(string))
        {
            throw new ArgumentException("This operator expects a string type property");
        }
    }

    protected static void ValidateForNumberAndDateTime()
    {
        Type numberType = typeof(INumber<>).MakeGenericType(typeof(TProperty));
        bool isNumberOrDate = typeof(TProperty).IsAssignableTo(numberType) || typeof(TProperty) == typeof(DateTime);

        if (!isNumberOrDate)
        {
            throw new ArgumentException("This operator expects a number or DateTime type property");
        }
    }
}