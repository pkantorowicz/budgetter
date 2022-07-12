using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using MediatR;

namespace Budgetter.Domain.Targets.DomainEvents;

public record TargetAttributesChangedDomainEvent : DomainEventBase, INotification
{
    public TargetAttributesChangedDomainEvent(
        Guid targetId,
        Guid budgetPlanId,
        string title,
        string description,
        DateTime validFrom,
        DateTime validTo,
        decimal maxAmount,
        string currency)
    {
        TargetId = targetId;
        BudgetPlanId = budgetPlanId;
        Title = title;
        Description = description;
        ValidFrom = validFrom;
        ValidTo = validTo;
        MaxAmount = maxAmount;
        Currency = currency;
    }

    public Guid TargetId { get; }
    public Guid BudgetPlanId { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }
    public decimal MaxAmount { get; }
    public string Currency { get; }
}