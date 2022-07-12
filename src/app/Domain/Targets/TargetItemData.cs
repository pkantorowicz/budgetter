using System;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Plans;

namespace Budgetter.Domain.Targets;

public record TargetItemData
{
    public TargetItemData(
        TargetItemId id,
        BudgetPlanId budgetPlanId,
        Title title,
        Money unitPrice,
        DateTime occurredAt)
    {
        Id = id;
        BudgetPlanId = budgetPlanId;
        Title = title;
        UnitPrice = unitPrice;
        OccurredAt = occurredAt;
    }

    public TargetItemId Id { get; }
    public BudgetPlanId BudgetPlanId { get; }
    public Title Title { get; }
    public Money UnitPrice { get; }
    public DateTime OccurredAt { get; }
}