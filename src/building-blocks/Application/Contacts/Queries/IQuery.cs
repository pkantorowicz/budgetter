using Budgetter.BuildingBlocks.Application.Contacts.Results;
using MediatR;

namespace Budgetter.BuildingBlocks.Application.Contacts.Queries;

public interface IQuery<out TResult> : IRequest<IQueryResult<TResult>>
{
}