using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.FinanceOperations.DomainEvents;

public record AssignationStatusChangedDomainEvent : DomainEventBase, INotification
{
    public AssignationStatusChangedDomainEvent(
        Guid financeOperationId, 
        Guid budgetPlanId, 
        Guid targetId, bool isAllocated)
    {
        FinanceOperationId = financeOperationId;
        BudgetPlanId = budgetPlanId;
        TargetId = targetId;
        IsAllocated = isAllocated;
    }

    public Guid FinanceOperationId { get; }
    public Guid BudgetPlanId { get; }
    public Guid TargetId { get; }
    public bool IsAllocated { get; }
}