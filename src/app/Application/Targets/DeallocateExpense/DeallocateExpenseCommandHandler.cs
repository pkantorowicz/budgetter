using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.FinanceOperations;
using Budgetter.Domain.Targets;

namespace Budgetter.Application.Targets.DeallocateExpense;

internal class DeallocateExpenseCommandHandler : CommandHandlerBase<DeallocateExpenseCommand>
{
    public DeallocateExpenseCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
    }

    public override async Task<ICommandResult> Handle(DeallocateExpenseCommand command,
        CancellationToken cancellationToken)
    {
        var target = await AggregateStore.Load(TargetId.New(command.TargetId));

        target.DeallocateExpense(FinanceOperationId.New(command.ExpenseId));

        AggregateStore.AppendChanges(target);

        return CommandResult.Success();
    }
}