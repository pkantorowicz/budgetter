namespace Budgetter.BuildingBlocks.Application.Contacts.Results;

public interface IQueryResult<out T> : IResult
{
    T Data { get; }
}