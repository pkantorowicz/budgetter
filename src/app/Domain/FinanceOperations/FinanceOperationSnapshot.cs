using System;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Plans;
using Budgetter.Domain.Targets;

namespace Budgetter.Domain.FinanceOperations;

public record FinanceOperationSnapshot
{
    public FinanceOperationSnapshot(
        FinanceOperationId financeOperationId,
        BudgetPlanId budgetPlanId,
        Title title,
        Money price,
        FinanceOperationType type,
        DateTime? occurredAt,
        TargetId targetId,
        bool isAllocated,
        bool isRemoved)
    {
        FinanceOperationId = financeOperationId;
        BudgetPlanId = budgetPlanId;
        Title = title;
        Price = price;
        Type = type;
        OccurredAt = occurredAt;
        TargetId = targetId;
        IsAllocated = isAllocated;
        IsRemoved = isRemoved;
    }

    public FinanceOperationId FinanceOperationId { get; }
    public BudgetPlanId BudgetPlanId { get; }
    public Title Title { get; }
    public Money Price { get; }
    public FinanceOperationType Type { get; }
    public DateTime? OccurredAt { get; }
    public TargetId TargetId { get; }
    public bool IsAllocated { get; }
    public bool IsRemoved { get; }
}