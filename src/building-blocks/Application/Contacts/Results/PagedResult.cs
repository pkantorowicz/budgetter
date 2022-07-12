using System.Collections.Generic;
using System.Linq;

namespace Budgetter.BuildingBlocks.Application.Contacts.Results;

public class PagedResult<TResult> : PagedResultBase
{
    private PagedResult()
    {
        Items = Enumerable.Empty<TResult>();
    }

    private PagedResult(IEnumerable<TResult> items,
        int currentPage,
        int resultsPerPage,
        int totalPages,
        long totalResults)
        : base(
            currentPage,
            resultsPerPage,
            totalPages,
            totalResults)
    {
        Items = items;
    }

    public IEnumerable<TResult> Items { get; }

    public bool IsEmpty => Items == null || !Items.Any();
    public bool IsNotEmpty => !IsEmpty;

    public static PagedResult<TResult> Empty => new();

    public static PagedResult<TResult> Create(
        IEnumerable<TResult> items,
        int currentPage,
        int resultsPerPage,
        int totalPages,
        long totalResults)
    {
        return new PagedResult<TResult>(
            items,
            currentPage,
            resultsPerPage,
            totalPages,
            totalResults);
    }

    public static PagedResult<TResult> From(PagedResultBase result, IEnumerable<TResult> items)
    {
        return new PagedResult<TResult>(items, result.CurrentPage, result.ResultsPerPage, result.TotalPages,
            result.TotalResults);
    }
}