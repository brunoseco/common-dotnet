using Common.Enum;
using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

public static class OrderReflection
{

    private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
    {
        ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
        MemberExpression property = Expression.PropertyOrField(param, propertyName);
        LambdaExpression sort = Expression.Lambda(property, param);

        MethodCallExpression call = Expression.Call(
            typeof(Queryable),
            (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
            new[] { typeof(T), property.Type },
            source.Expression,
            Expression.Quote(sort));

        return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);

    }
    private static IQueryable<T> ThenBy<T>(this IQueryable<T> source, string propertyName)
    {
        return OrderingHelper(source, propertyName, false, true);
    }
    private static IQueryable<T> ThenByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return OrderingHelper(source, propertyName, true, true);
    }
}

