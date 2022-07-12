using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.Plans.DomainEvents;

public record BudgetPlanCreatedDomainEvent : DomainEventBase, INotification
{
    public BudgetPlanCreatedDomainEvent(
        Guid budgetPlanId,
        string title,
        string currency,
        DateTime validFrom,
        DateTime validTo)
    {
        BudgetPlanId = budgetPlanId;
        Title = title;
        Currency = currency;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public Guid BudgetPlanId { get; }
    public string Title { get; }
    public string Currency { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }
}