using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.StateMachines;

internal interface IBudgetPlanOperationState
{
    void Process(BudgetPlanSelectionViewModel budgetPlanSelectionViewModel);
}