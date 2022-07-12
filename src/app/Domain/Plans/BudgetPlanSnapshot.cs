using System;
using Budgetter.Domain.Commons.ValueObjects;

namespace Budgetter.Domain.Plans;

public class BudgetPlanSnapshot
{
    public BudgetPlanSnapshot(
        Guid id,
        string currency,
        Duration duration,
        Title title,
        bool isRemoved)
    {
        Id = id;
        Currency = currency;
        Duration = duration;
        Title = title;
        IsRemoved = isRemoved;
    }

    public Guid Id { get; }
    public string Currency { get; }
    public Duration Duration { get; }
    public Title Title { get; }
    public bool IsRemoved { get; }
}