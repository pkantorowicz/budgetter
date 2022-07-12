using System.Collections.Generic;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Plans;

namespace Budgetter.Domain.Targets;

public record TargetSnapshot
{
    public TargetSnapshot(
        TargetId targetId,
        BudgetPlanId budgetPlanId,
        Title title,
        Description description,
        Money maxAmount,
        Duration duration,
        Status status,
        TargetBalance balance,
        IEnumerable<TargetItemData> expenses,
        bool isRemoved)
    {
        TargetId = targetId;
        BudgetPlanId = budgetPlanId;
        Title = title;
        Description = description;
        MaxAmount = maxAmount;
        Duration = duration;
        Status = status;
        Balance = balance;
        Expenses = expenses;
        IsRemoved = isRemoved;
    }

    public TargetId TargetId { get; }
    public BudgetPlanId BudgetPlanId { get; }
    public Title Title { get; }
    public Description Description { get; }
    public Money MaxAmount { get; }
    public Duration Duration { get; }
    public Status Status { get; }
    public TargetBalance Balance { get; }
    public IEnumerable<TargetItemData> Expenses { get; }
    public bool IsRemoved { get; }
}