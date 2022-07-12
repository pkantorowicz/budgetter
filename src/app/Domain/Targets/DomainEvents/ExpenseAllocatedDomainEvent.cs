using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.Targets.DomainEvents;

public record ExpenseAllocatedDomainEvent : DomainEventBase, INotification
{
    public ExpenseAllocatedDomainEvent(
        Guid targetId,
        Guid budgetPlanId,
        Guid expenseId,
        string title,
        decimal unitPrice,
        string currency,
        DateTime occurredAt)
    {
        TargetId = targetId;
        BudgetPlanId = budgetPlanId;
        ExpenseId = expenseId;
        Title = title;
        UnitPrice = unitPrice;
        Currency = currency;
        OccurredAt = occurredAt;
    }

    public Guid TargetId { get; }
    public Guid BudgetPlanId { get; }
    public Guid ExpenseId { get; }
    public string Title { get; }
    public decimal UnitPrice { get; }
    public string Currency { get; }
    public DateTime OccurredAt { get; }
}