using System;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Events;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.StateMachines.States;

internal class BudgetPlanCreationCancelledState : IBudgetPlanOperationState
{
    private readonly EventHandler<GoBackEventArgs> _goBackEventHandler;
    private readonly BudgetPlanViewModel _previouslySelectedBudgetPlan;

    public BudgetPlanCreationCancelledState(
        EventHandler<GoBackEventArgs> goBackEventHandler,
        BudgetPlanViewModel previouslySelectedBudgetPlan)
    {
        _goBackEventHandler = goBackEventHandler;
        _previouslySelectedBudgetPlan = previouslySelectedBudgetPlan;
    }

    public void Process(BudgetPlanSelectionViewModel budgetPlanSelectionViewModel)
    {
        if (budgetPlanSelectionViewModel?.SelectedBudgetPlan is null)
            return;

        budgetPlanSelectionViewModel.SelectedBudgetPlan.Dispose();
        budgetPlanSelectionViewModel.SelectedBudgetPlan.GoBack -= _goBackEventHandler;
        budgetPlanSelectionViewModel.SelectedBudgetPlan = _previouslySelectedBudgetPlan;
    }
}