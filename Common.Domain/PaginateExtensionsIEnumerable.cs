using Common.Domain;
using Common.Domain.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class PaginateExtensionsIEnumerable
{
    public static PaginateResult<T> PaginateNew<T>(this IEnumerable<T> querySorted, IEnumerable<T> query, Filter filter, int totalCount = 0) where T : class
    {
        var paginationResult = new PaginatedDataIEnumerable<T>(querySorted, filter, totalCount);

        return new PaginateResult<T>
        {
            ResultPaginatedData = paginationResult,
            TotalCount = paginationResult.TotalCount,
            Source = query.AsQueryable()
        };
    }

    public static PaginateResult<T> PaginateNew<T>(this IEnumerable<T> source, Filter filter) where T : class
    {
        var paginationResult = new PaginatedDataIEnumerable<T>(source, filter);

        return new PaginateResult<T>
        {
            ResultPaginatedData = paginationResult,
            TotalCount = paginationResult.TotalCount,
            Source = source.AsQueryable()
        };
    }

    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, Filter filter) where T : class
    {
        var paginationResult = new PaginatedDataIEnumerable<T>(source, filter);
        return paginationResult;
    }
    
}








