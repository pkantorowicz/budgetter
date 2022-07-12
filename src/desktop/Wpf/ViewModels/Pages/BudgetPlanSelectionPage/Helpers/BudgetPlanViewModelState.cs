using System;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers;

public record BudgetPlanViewModelState
{
    public BudgetPlanViewModelState(
        string title,
        string currency,
        DateTime? validFrom,
        DateTime? validTo)
    {
        Title = title;
        Currency = currency;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public string Title { get; }
    public string Currency { get; }
    public DateTime? ValidFrom { get; }
    public DateTime? ValidTo { get; }
}