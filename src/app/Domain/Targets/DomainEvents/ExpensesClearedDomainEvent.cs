using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.Targets.DomainEvents;

public record ExpensesClearedDomainEvent : DomainEventBase, INotification
{
    public ExpensesClearedDomainEvent(
        Guid targetId,
        Guid budgetPlanId)
    {
        TargetId = targetId;
        BudgetPlanId = budgetPlanId;
    }

    public Guid TargetId { get; }
    public Guid BudgetPlanId { get; }
}