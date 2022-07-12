using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;

namespace Budgetter.BuildingBlocks.Application.Contacts.Commands;

public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    protected readonly IAggregateStore AggregateStore;

    protected CommandHandlerBase(IAggregateStore aggregateStore)
    {
        AggregateStore = aggregateStore;
    }

    public abstract Task<ICommandResult> Handle(TCommand request, CancellationToken cancellationToken);
}

public abstract class CommandHandlerBase<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    protected IAggregateStore AggregateStore;

    protected CommandHandlerBase(IAggregateStore aggregateStore)
    {
        AggregateStore = aggregateStore;
    }

    public abstract Task<ICommandResult<TResult>> Handle(TCommand request, CancellationToken cancellationToken);
}