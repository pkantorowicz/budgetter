using Budgetter.BuildingBlocks.Application.Contacts.Results;
using MediatR;

namespace Budgetter.BuildingBlocks.Application.Contacts.Commands;

public interface ICommandHandler<in TCommand> :
    IRequestHandler<TCommand, ICommandResult>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResult> :
    IRequestHandler<TCommand, ICommandResult<TResult>>
    where TCommand : ICommand<TResult>
{
}