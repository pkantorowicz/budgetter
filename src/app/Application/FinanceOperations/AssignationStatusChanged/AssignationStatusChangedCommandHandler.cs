using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Application.Exceptions;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.FinanceOperations;

namespace Budgetter.Application.FinanceOperations.AssignationStatusChanged;

public class AssignationStatusChangedCommandHandler : CommandHandlerBase<AssignationStatusChangedCommand>
{
    public AssignationStatusChangedCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
    }

    public override async Task<ICommandResult> Handle(AssignationStatusChangedCommand command,
        CancellationToken cancellationToken)
    {
        var financeOperation = await AggregateStore.Load(
            FinanceOperationId.New(
                command.FinanceOperationId));

        if (financeOperation is null)
            throw new RecordNotFoundException(
                $"Finance operation with provided id: {command.FinanceOperationId} doesn't exists.");

        if (command.AssignationStatus)
            financeOperation.MarkAsAssigned(command.TargetId);
        else
            financeOperation.MarkAsUnAssigned();

        AggregateStore.AppendChanges(financeOperation);

        return CommandResult.Success();
    }
}