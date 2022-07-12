using Budgetter.BuildingBlocks.Application.Contacts.Results;
using MediatR;

namespace Budgetter.BuildingBlocks.Application.Contacts.Commands;

public interface ICommand : IRequest<ICommandResult>
{
}

public interface ICommand<out TResult> : IRequest<ICommandResult<TResult>>
{
}