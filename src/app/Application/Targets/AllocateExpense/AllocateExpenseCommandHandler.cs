using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.FinanceOperations;
using Budgetter.Domain.Targets;
using Budgetter.Domain.Targets.Services.Interfaces;

namespace Budgetter.Application.Targets.AllocateExpense;

internal class AllocateExpenseCommandHandler : CommandHandlerBase<AllocateExpenseCommand>
{
    private readonly IFinanceOperationExistenceChecker _financeOperationExistenceChecker;

    public AllocateExpenseCommandHandler(IAggregateStore aggregateStore,
        IFinanceOperationExistenceChecker financeOperationExistenceChecker) : base(aggregateStore)
    {
        _financeOperationExistenceChecker = financeOperationExistenceChecker;
    }

    public override async Task<ICommandResult> Handle(AllocateExpenseCommand command,
        CancellationToken cancellationToken)
    {
        var target = await AggregateStore.Load(TargetId.New(command.TargetId));

        target.AllocateExpense(
            FinanceOperationId.New(command.FinanceOperation.Id),
            Title.Entitle(command.FinanceOperation.Title),
            Money.Of(
                command.FinanceOperation.Price,
                command.FinanceOperation.Currency),
            command.FinanceOperation.OccurredAt,
            _financeOperationExistenceChecker);

        AggregateStore.AppendChanges(target);

        return CommandResult.Success();
    }
}