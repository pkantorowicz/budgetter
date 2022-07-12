using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.FinanceOperations.DomainEvents;

public record FinanceOperationRemovedDomainEvent : DomainEventBase, INotification
{
    public FinanceOperationRemovedDomainEvent(
        Guid financeOperationId,
        Guid budgetPlanId,
        bool isRemoved)
    {
        FinanceOperationId = financeOperationId;
        BudgetPlanId = budgetPlanId;
        IsRemoved = isRemoved;
    }

    public Guid FinanceOperationId { get; }
    public Guid BudgetPlanId { get; }
    public bool IsRemoved { get; }
}