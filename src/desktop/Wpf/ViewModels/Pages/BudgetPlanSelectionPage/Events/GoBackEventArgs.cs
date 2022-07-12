using System;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Events;

internal class GoBackEventArgs : EventArgs
{
    public GoBackEventArgs(
        BudgetPlanChangesMode budgetPlanChangesMode,
        bool cancelled)
    {
        BudgetPlanChangesMode = budgetPlanChangesMode;
        Cancelled = cancelled;
    }

    public BudgetPlanChangesMode BudgetPlanChangesMode { get; }
    public bool Cancelled { get; }
}