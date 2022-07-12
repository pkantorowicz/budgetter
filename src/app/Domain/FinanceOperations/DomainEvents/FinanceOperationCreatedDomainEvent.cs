using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.FinanceOperations.DomainEvents;

public record FinanceOperationCreatedDomainEvent : DomainEventBase, INotification
{
    public FinanceOperationCreatedDomainEvent(
        Guid financeOperationId,
        Guid budgetPlanId,
        string title,
        decimal price,
        string currency,
        string type,
        DateTime? occurredAt)
    {
        FinanceOperationId = financeOperationId;
        BudgetPlanId = budgetPlanId;
        Title = title;
        Price = price;
        Currency = currency;
        Type = type;
        OccurredAt = occurredAt;
    }

    public Guid FinanceOperationId { get; }
    public Guid BudgetPlanId { get; }
    public string Title { get; }
    public decimal Price { get; }
    public string Currency { get; }
    public string Type { get; }
    public DateTime? OccurredAt { get; }
}