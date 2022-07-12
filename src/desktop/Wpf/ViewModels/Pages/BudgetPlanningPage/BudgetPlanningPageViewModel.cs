using System.Threading.Tasks;
using System.Windows.Input;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Events;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Helpers;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Controls;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage;

internal class BudgetPlanningPageViewModel : ViewModelBase, IPageViewModel
{
    private readonly IBudgetPlanContext _budgetPlanContext;
    private readonly IEventAggregator _eventAggregator;
    private readonly IFinanceOperationsController _financeOperationsController;
    private readonly ITargetsController _targetsController;

    public BudgetPlanningPageViewModel(
        IEventAggregator eventAggregator,
        IBudgetPlanContext budgetPlanContext,
        IFinanceOperationsController financeOperationsController,
        ITargetsController targetsController)
    {
        _eventAggregator = eventAggregator;
        _budgetPlanContext = budgetPlanContext;
        _financeOperationsController = financeOperationsController;
        _targetsController = targetsController;

        GoBackCommand = new RelayCommand(GoBack);
        GoNextCommand = new RelayCommand(GoNext);

        BudgetPlanningPageExpensesListViewModel =
            new BudgetPlanningPageExpensesListViewModel(
                budgetPlanContext,
                financeOperationsController,
                _eventAggregator);

        BudgetPlanningPageTargetListViewModel =
            new BudgetPlanningPageTargetListViewModel(
                _budgetPlanContext,
                _targetsController,
                _eventAggregator,
                _financeOperationsController);
    }

    public ICommand GoBackCommand { get; }
    public ICommand GoNextCommand { get; }

    public BudgetPlanningPageExpensesListViewModel BudgetPlanningPageExpensesListViewModel { get; }
    public BudgetPlanningPageTargetListViewModel BudgetPlanningPageTargetListViewModel { get; }

    public async Task OnActivateAsync()
    {
        await BudgetPlanningPageTargetListViewModel.GetTargets();
    }

    public async Task OnDeactivateAsync()
    {
        await Task.CompletedTask;
    }

    public override void Dispose()
    {
    }

    private async void GoBack(object obj)
    {
        await _eventAggregator.PublishOnUIThreadAsync(NavigationDirection.FinanceOperationsPage);
    }

    private async void GoNext(object obj)
    {
        // await _eventAggregator.PublishOnUIThreadAsync(NavigationDirection.BudgetPlanningPage);
    }
}