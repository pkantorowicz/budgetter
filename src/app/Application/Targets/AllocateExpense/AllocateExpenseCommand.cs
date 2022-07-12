using System;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.Targets.AllocateExpense;

public record AllocateExpenseCommand : CommandBase
{
    public AllocateExpenseCommand(
        Guid targetId,
        FinanceOperationDto financeOperation)
    {
        TargetId = targetId;
        FinanceOperation = financeOperation;
    }

    public Guid TargetId { get; }
    public FinanceOperationDto FinanceOperation { get; }
}