using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.FinanceOperations.DomainEvents;

public record FinanceOperationAttributesChangedDomainEvent : DomainEventBase, INotification
{
    public FinanceOperationAttributesChangedDomainEvent(
        Guid financeOperationId,
        Guid budgetPlanId,
        string title,
        decimal price)
    {
        FinanceOperationId = financeOperationId;
        BudgetPlanId = budgetPlanId;
        Title = title;
        Price = price;
    }

    public Guid FinanceOperationId { get; }
    public Guid BudgetPlanId { get; }
    public string Title { get; }
    public decimal Price { get; }
}