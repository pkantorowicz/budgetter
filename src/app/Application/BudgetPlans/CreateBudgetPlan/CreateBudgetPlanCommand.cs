using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.BudgetPlans.CreateBudgetPlan;

public record CreateBudgetPlanCommand : CommandBase<Guid>
{
    public CreateBudgetPlanCommand(
        string title,
        string currency,
        DateTime validFrom,
        DateTime validTo,
        int maxDaysCount)
    {
        Title = title;
        Currency = currency;
        ValidFrom = validFrom;
        ValidTo = validTo;
        MaxDaysCount = maxDaysCount;
    }

    public string Title { get; }
    public string Currency { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }
    public int MaxDaysCount { get; }
}