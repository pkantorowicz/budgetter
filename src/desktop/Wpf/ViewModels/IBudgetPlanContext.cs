using System;

namespace Budgetter.Wpf.ViewModels;

internal interface IBudgetPlanContext
{
    Guid BudgetPlanId { get; }
    string Title { get; }
    string Currency { get; }
    DateTime? ValidFrom { get; }
    DateTime? ValidTo { get; }

    void ModifyState(Guid budgetPlanId,
        string title,
        string currency,
        DateTime? validFrom,
        DateTime? validTo);
}