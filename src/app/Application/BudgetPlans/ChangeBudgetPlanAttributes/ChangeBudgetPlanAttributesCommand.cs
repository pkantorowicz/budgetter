using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.BudgetPlans.ChangeBudgetPlanAttributes;

public record ChangeBudgetPlanAttributesCommand : CommandBase
{
    public ChangeBudgetPlanAttributesCommand(
        Guid budgetPlanId,
        string title,
        string currency,
        DateTime validFrom,
        DateTime validTo,
        int maxDaysCount)
    {
        BudgetPlanId = budgetPlanId;
        Title = title;
        Currency = currency;
        ValidFrom = validFrom;
        ValidTo = validTo;
        MaxDaysCount = maxDaysCount;
    }

    public Guid BudgetPlanId { get; }
    public string Title { get; }
    public string Currency { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }
    public int MaxDaysCount { get; }
}