using System;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Events;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.StateMachines.States;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.StateMachines;

internal class BudgetPlanOperationStateCoordinator
{
    private readonly BudgetPlanCreatedState _budgetPlanCreatedState;
    private readonly BudgetPlanCreationCancelledState _budgetPlanCreationCancelledState;
    private readonly BudgetPlanModificationCancelledState _budgetPlanModificationCancelledState;
    private readonly BudgetPlanModifiedState _budgetPlanModifiedState;

    public BudgetPlanOperationStateCoordinator(
        EventHandler<GoBackEventArgs> goBackEventHandler,
        BudgetPlanViewModel previouslySelectedBudgetPlan,
        BudgetPlanViewModel currentlySelectedBudgetPlan,
        BudgetPlanViewModelState budgetPlanViewModelState)
    {
        _budgetPlanCreationCancelledState =
            new BudgetPlanCreationCancelledState(goBackEventHandler, previouslySelectedBudgetPlan);
        _budgetPlanCreatedState = new BudgetPlanCreatedState(currentlySelectedBudgetPlan);
        _budgetPlanModificationCancelledState = new BudgetPlanModificationCancelledState(budgetPlanViewModelState);
        _budgetPlanModifiedState = new BudgetPlanModifiedState();
    }

    public IBudgetPlanOperationState GetCurrentState(BudgetPlanChangesMode mode, bool cancelled)
    {
        return mode switch
        {
            BudgetPlanChangesMode.Create when cancelled => _budgetPlanCreationCancelledState,
            BudgetPlanChangesMode.Create => _budgetPlanCreatedState,
            BudgetPlanChangesMode.Modify when cancelled => _budgetPlanModificationCancelledState,
            BudgetPlanChangesMode.Modify => _budgetPlanModifiedState,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown BudgetPlanChangesMode.")
        };
    }
}