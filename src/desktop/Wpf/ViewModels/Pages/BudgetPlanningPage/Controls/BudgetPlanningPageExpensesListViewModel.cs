using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework.Events;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Controls;

internal class BudgetPlanningPageExpensesListViewModel : ViewModelBase, IHandle<object>
{
    private readonly IBudgetPlanContext _budgetPlanContext;
    private readonly IEventAggregator _eventAggregator;

    private ObservableCollection<BudgetPlanningPageExpenseViewModel> _expenses;

    public BudgetPlanningPageExpensesListViewModel(
        IBudgetPlanContext budgetPlanContext,
        IFinanceOperationsController financeOperationsController,
        IEventAggregator eventAggregator)
    {
        _budgetPlanContext = budgetPlanContext;
        _eventAggregator = eventAggregator;

        _eventAggregator.SubscribeOnUIThread(this);

        SelectedExpenses = new ObservableCollection<BudgetPlanningPageExpenseViewModel>();
        
        Task.Run(() => GetExpenses(financeOperationsController));
    }

    public ObservableCollection<BudgetPlanningPageExpenseViewModel> Expenses
    {
        get => _expenses;
        set
        {
            _expenses = value;

            OnPropertyChanged();
        }
    }

    public ObservableCollection<BudgetPlanningPageExpenseViewModel> SelectedExpenses { get; }

    public async Task HandleAsync(object message, CancellationToken cancellationToken)
    {
        if (message is ExpensesAllocatedEvent expensesAllocated)
        {
            foreach (var allocatedExpense in expensesAllocated.AllocatedExpenses)
                Expenses.Remove(allocatedExpense);

            SelectedExpenses.Clear();
        }

        await Task.CompletedTask;
    }

    public override void Dispose()
    {
        if (Expenses == null) return;
        if (!Expenses.Any()) return;

        foreach (var expense in Expenses)
            expense.Dispose();

        Expenses.Clear();
        Expenses = null;
    }

    private async Task GetExpenses(
        IFinanceOperationsController financeOperationsController)
    {
        var financeOperations =
            await financeOperationsController.FilterFinanceOperationsAsync(
                _budgetPlanContext.BudgetPlanId,
                null,
                null,
                null,
                null,
                null,
                false);

        Expenses = new ObservableCollection<BudgetPlanningPageExpenseViewModel>(financeOperations
            .Select(BudgetPlanningPageExpenseViewModel.CreateFromDto)
            .OrderByDescending(fo => fo.OccurredAt)
            .ToList());
    }
}