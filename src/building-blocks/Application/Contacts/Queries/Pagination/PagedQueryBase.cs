namespace Budgetter.BuildingBlocks.Application.Contacts.Queries.Pagination;

public record PagedQueryBase<TResult> : QueryBase<TResult>, IPagedQuery<TResult>
{
    public PagedQueryBase(
        int page,
        int results,
        string orderBy,
        SortDirection sortDirection)
    {
        Page = page;
        Results = results;
        OrderBy = orderBy;
        SortDirection = sortDirection;
    }

    public int Page { get; }
    public int Results { get; }
    public string OrderBy { get; }
    public SortDirection SortDirection { get; }
}