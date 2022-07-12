using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.Targets.DomainEvents;

public record TargetRemovedDomainEvent : DomainEventBase, INotification
{
    public TargetRemovedDomainEvent(
        Guid budgetPlanId,
        Guid targetId,
        bool isRemoved)
    {
        BudgetPlanId = budgetPlanId;
        TargetId = targetId;
        IsRemoved = isRemoved;
    }

    public Guid BudgetPlanId { get; }
    public Guid TargetId { get; }
    public bool IsRemoved { get; }
}