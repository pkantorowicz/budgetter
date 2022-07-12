using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.Targets.DomainEvents;

public record ExpenseDeallocatedDomainEvent : DomainEventBase, INotification
{
    public ExpenseDeallocatedDomainEvent(
        Guid targetId,
        Guid budgetPlanId,
        Guid expenseId)
    {
        TargetId = targetId;
        BudgetPlanId = budgetPlanId;
        ExpenseId = expenseId;
    }

    public Guid TargetId { get; }
    public Guid BudgetPlanId { get; }
    public Guid ExpenseId { get; }
}