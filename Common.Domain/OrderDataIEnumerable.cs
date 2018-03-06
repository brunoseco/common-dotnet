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

public static class OrderDataIEnumerable
{
    private static IEnumerable<T> OrderingHelperDynamic<T>(IEnumerable<T> source, string expression)
    {
        return source.OrderBy(expression);
    }
    private static IEnumerable<T> OrderByExpression<T>(this IEnumerable<T> source, string propertyName)
    {
        var expression = string.Format("{0}", propertyName);
        return OrderingHelperDynamic(source, expression);
    }
    private static IEnumerable<T> OrderByDescendingExpression<T>(this IEnumerable<T> source, string propertyName)
    {
        var expression = string.Format("{0} Descending", propertyName);
        return OrderingHelperDynamic(source, expression);
    }
    public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, Filter filters)
    {
        var sourceOrdering = default(IEnumerable<T>);

        if (filters.OrderFields.IsNotNull())
        {

            if (filters.orderByType == OrderByType.OrderBy)
                sourceOrdering = OrderByExpression(source, string.Join(",", filters.OrderFields));

            if (filters.orderByType == OrderByType.OrderByDescending)
                sourceOrdering = OrderByDescendingExpression(source, string.Join(",", filters.OrderFields));

        }
        else
            sourceOrdering = OrderByExpression(source, "1");

        return sourceOrdering.IsNotNull() ? sourceOrdering : source;
    }
}

