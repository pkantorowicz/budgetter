using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.StateMachines.States;

internal class BudgetPlanModificationCancelledState : IBudgetPlanOperationState
{
    private readonly BudgetPlanViewModelState _budgetPlanViewModelState;

    public BudgetPlanModificationCancelledState(BudgetPlanViewModelState budgetPlanViewModelState)
    {
        _budgetPlanViewModelState = budgetPlanViewModelState;
    }

    public void Process(BudgetPlanSelectionViewModel budgetPlanSelectionViewModel)
    {
        var selectedBudgetPlan = budgetPlanSelectionViewModel?.SelectedBudgetPlan;

        if (selectedBudgetPlan is null)
            return;

        selectedBudgetPlan.DisableValidation();
        selectedBudgetPlan.RestoreChanges(_budgetPlanViewModelState);
        selectedBudgetPlan.EnableValidation();
    }
}