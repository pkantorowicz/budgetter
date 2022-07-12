using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.Plans.DomainEvents;

public record BudgetPlanRemovedDomainEvent : DomainEventBase, INotification
{
    public BudgetPlanRemovedDomainEvent(
        Guid budgetPlanId,
        bool isRemoved)
    {
        BudgetPlanId = budgetPlanId;
        IsRemoved = isRemoved;
    }

    public Guid BudgetPlanId { get; }
    public bool IsRemoved { get; }
}