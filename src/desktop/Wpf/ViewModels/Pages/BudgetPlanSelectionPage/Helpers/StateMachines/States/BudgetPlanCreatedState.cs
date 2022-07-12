using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.StateMachines.States;

internal class BudgetPlanCreatedState : IBudgetPlanOperationState
{
    private readonly BudgetPlanViewModel _currentlySelectedBudgetPlan;

    public BudgetPlanCreatedState(BudgetPlanViewModel currentlySelectedBudgetPlan)
    {
        _currentlySelectedBudgetPlan = currentlySelectedBudgetPlan;
    }

    public void Process(BudgetPlanSelectionViewModel budgetPlanSelectionViewModel)
    {
        budgetPlanSelectionViewModel.BudgetPlans.Add(_currentlySelectedBudgetPlan);
    }
}