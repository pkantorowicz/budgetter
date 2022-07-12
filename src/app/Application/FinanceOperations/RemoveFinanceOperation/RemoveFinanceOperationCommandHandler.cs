using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.FinanceOperations;

namespace Budgetter.Application.FinanceOperations.RemoveFinanceOperation;

public class RemoveFinanceOperationCommandHandler : CommandHandlerBase<RemoveFinanceOperationCommand>
{
    private readonly IAggregateStore _aggregateStore;

    public RemoveFinanceOperationCommandHandler(
        IAggregateStore aggregateStore) : base(aggregateStore)
    {
        _aggregateStore = aggregateStore;
    }

    public override async Task<ICommandResult> Handle(RemoveFinanceOperationCommand command,
        CancellationToken cancellationToken)
    {
        var financeOperation = await AggregateStore.Load(FinanceOperationId.New(command.FinanceOperationId));

        financeOperation.MarkAsRemoved();

        _aggregateStore.AppendChanges(financeOperation);

        return CommandResult.Success();
    }
}