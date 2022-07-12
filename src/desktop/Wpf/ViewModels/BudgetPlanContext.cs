using System;

namespace Budgetter.Wpf.ViewModels;

internal record BudgetPlanContext : IBudgetPlanContext
{
    private BudgetPlanContext()
    {
    }

    public static BudgetPlanContext Default => new();

    public Guid BudgetPlanId { get; private set; }
    public string Title { get; private set; }
    public string Currency { get; private set; }
    public DateTime? ValidFrom { get; private set; }
    public DateTime? ValidTo { get; private set; }

    public void ModifyState(
        Guid budgetPlanId,
        string title,
        string currency,
        DateTime? validFrom,
        DateTime? validTo)
    {
        BudgetPlanId = budgetPlanId;
        Title = title;
        Currency = currency;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }
}