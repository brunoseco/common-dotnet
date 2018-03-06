using Common.Domain;
using Common.Domain.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class PaginateExtensionsIQueryable
{
    public static PaginateResult<T> PaginateNew<T>(this IQueryable<T> querySorted, IQueryable<T> query, Filter filter, int totalCount = 0) where T : class
    {
        var paginationResult = new PaginatedDataIQueryable<T>(querySorted, filter, totalCount);

        return new PaginateResult<T>
        {
            ResultPaginatedData = paginationResult,
            TotalCount = paginationResult.TotalCount,
            Source = query
        };
    }

    public static PaginateResult<T> PaginateNew<T>(this IQueryable<T> source, Filter filter) where T : class
    {
        var paginationResult = new PaginatedDataIQueryable<T>(source, filter);

        return new PaginateResult<T>
        {
            ResultPaginatedData = paginationResult,
            TotalCount = paginationResult.TotalCount,
            Source = source
        };
    }

    public static IEnumerable<T> Paginate<T>(this IQueryable<T> source, Filter filter) where T : class
    {
        var paginationResult = new PaginatedDataIQueryable<T>(source, filter);
        return paginationResult;
    }
    
}








