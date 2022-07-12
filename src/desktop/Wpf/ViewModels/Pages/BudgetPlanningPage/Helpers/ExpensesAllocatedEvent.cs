using System.Collections.Generic;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Controls;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Helpers;

internal record ExpensesAllocatedEvent
{
    public ExpensesAllocatedEvent(IEnumerable<BudgetPlanningPageExpenseViewModel> allocatedExpenses)
    {
        AllocatedExpenses = allocatedExpenses;
    }

    public IEnumerable<BudgetPlanningPageExpenseViewModel> AllocatedExpenses { get; }
}