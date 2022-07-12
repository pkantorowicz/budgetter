using System;
using System.Collections.Generic;
using System.Linq;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Extensions;

namespace Budgetter.BuildingBlocks.Application.Contacts.Queries.Pagination;

public static class PaginationExtensions
{
    public static PagedResult<T> Paginate<T>(this IEnumerable<T> values)
    {
        return values.Paginate(1, int.MaxValue);
    }

    public static PagedResult<T> Paginate<T, TResult>(this IEnumerable<T> values, IPagedQuery<TResult> query)
    {
        return values.Paginate(query.Page, query.Results, query.OrderBy, query.SortDirection);
    }

    private static PagedResult<T> Paginate<T>(
        this IEnumerable<T> values,
        int page,
        int resultsPerPage = 10,
        string orderBy = null,
        SortDirection sortOrder = SortDirection.Ascending)
    {
        if (page <= 0)
            page = 1;

        if (resultsPerPage <= 0)
            resultsPerPage = 10;

        var valuesAsList = values.ToList();
        var isEmpty = valuesAsList.Any() == false;

        if (isEmpty)
            return PagedResult<T>.Empty;

        var totalResults = valuesAsList.Count;
        var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
        var data = valuesAsList.Limit(page, resultsPerPage, orderBy, sortOrder).ToList();

        return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
    }

    public static IEnumerable<T> Limit<T, TResult>(
        this IEnumerable<T> collection,
        IPagedQuery<TResult> query)
    {
        return collection.Limit(query.Page, query.Results);
    }

    private static IEnumerable<T> Limit<T>(
        this IEnumerable<T> collection,
        int page = 1,
        int resultsPerPage = 10,
        string orderBy = null,
        SortDirection sortOrder = SortDirection.Ascending)
    {
        if (page <= 0)
            page = 1;

        if (resultsPerPage <= 0)
            resultsPerPage = 10;

        var skip = (page - 1) * resultsPerPage;


        var data = collection.Skip(skip)
            .Take(resultsPerPage);

        if (string.IsNullOrEmpty(orderBy))
            return data;

        return sortOrder switch
        {
            SortDirection.Ascending => data
                .AsQueryable()
                .OrderBy(orderBy),

            SortDirection.Descending =>
                data
                    .AsQueryable()
                    .OrderByDescending(orderBy),

            _ => throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, "Unknown sort direction.")
        };
    }
}