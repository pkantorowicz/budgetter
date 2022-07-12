using Budgetter.BuildingBlocks.Application.Contacts.Results;
using MediatR;

namespace Budgetter.BuildingBlocks.Application.Contacts.Queries;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, IQueryResult<TResult>>
    where TQuery : IQuery<TResult>
{
}