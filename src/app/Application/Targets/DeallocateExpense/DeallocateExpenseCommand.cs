using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.Targets.DeallocateExpense;

public record DeallocateExpenseCommand : CommandBase
{
    public DeallocateExpenseCommand(
        Guid targetId,
        Guid expenseId)
    {
        TargetId = targetId;
        ExpenseId = expenseId;
    }

    public Guid TargetId { get; }
    public Guid ExpenseId { get; }
}