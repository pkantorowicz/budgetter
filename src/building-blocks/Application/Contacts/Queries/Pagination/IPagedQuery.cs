namespace Budgetter.BuildingBlocks.Application.Contacts.Queries.Pagination;

public interface IPagedQuery<out TResult> : IQuery<TResult>
{
    int Page { get; }
    int Results { get; }
    string OrderBy { get; }
    SortDirection SortDirection { get; }
}