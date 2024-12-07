using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.OCP;
internal static class ExpressionUtils
{
    internal static Expression LikeExpression(Expression matchPropertyExpression, Expression patternExpression)
    {
        return Expression.Call(
            typeof(DbFunctionsExtensions),
            nameof(DbFunctionsExtensions.Like),
            null,
            Expression.Constant(EF.Functions),
            matchPropertyExpression,
            patternExpression
            );
    }
}