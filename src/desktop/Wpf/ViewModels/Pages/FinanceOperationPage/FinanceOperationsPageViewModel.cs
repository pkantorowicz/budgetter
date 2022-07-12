using System.Threading.Tasks;
using System.Windows.Input;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Events;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Controls;
using Budgetter.Wpf.ViewModels.Helpers;
using Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Controls;
using Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage;

internal class FinanceOperationsPageViewModel : ViewModelBase, IPageViewModel
{
    private readonly IEventAggregator _eventAggregator;
    private FinanceOperationsPageFiltersViewModel _filters;
    private FinanceOperationsPageListViewModel _financeOperationsList;

    private FilterChangesEventArgs _lastFilterChangesEventArgs;
    private SearchViewModel _search;

    public FinanceOperationsPageViewModel(
        IEventAggregator eventAggregator,
        IFinanceOperationsController financeOperationsController,
        IBudgetPlanContext budgetPlanContext)
    {
        _eventAggregator = eventAggregator;

        BudgetPlanContext = budgetPlanContext;
        GoBackCommand = new RelayCommand(GoBack);
        GoNextCommand = new RelayCommand(GoNext);

        FinanceOperationsList = new FinanceOperationsPageListViewModel(
            budgetPlanContext,
            financeOperationsController);

        Filters = new FinanceOperationsPageFiltersViewModel(budgetPlanContext);
        Filters.FiltersChanged += OnFiltersChanged;

        Search = new SearchViewModel();
        Search.SearchTextChanged += OnSearchTextChanged;
    }

    public ICommand GoBackCommand { get; }
    public ICommand GoNextCommand { get; }
    public IBudgetPlanContext BudgetPlanContext { get; }

    public FinanceOperationsPageFiltersViewModel Filters
    {
        get => _filters;
        set
        {
            _filters = value;

            OnPropertyChanged();
        }
    }

    public FinanceOperationsPageListViewModel FinanceOperationsList
    {
        get => _financeOperationsList;
        set
        {
            _financeOperationsList = value;

            OnPropertyChanged();
        }
    }

    public SearchViewModel Search
    {
        get => _search;
        set
        {
            _search = value;

            OnPropertyChanged();
        }
    }

    public async Task OnActivateAsync()
    {
        await FinanceOperationsList.RefreshFinanceOperations();
    }

    public async Task OnDeactivateAsync()
    {
        Dispose();

        await Task.CompletedTask;
    }

    public override void Dispose()
    {
        if (FinanceOperationsList != null)
        {
            FinanceOperationsList.Dispose();
            FinanceOperationsList = null;
        }

        if (Filters != null)
        {
            Filters.FiltersChanged -= OnFiltersChanged;
            Filters.Dispose();
            Filters = null;
        }

        if (Search != null)
        {
            Search.SearchTextChanged -= OnSearchTextChanged;
            Search.Dispose();
            Search = null;
        }
    }

    private async void OnFiltersChanged(object sender, FilterChangesEventArgs lastFilterChangesEventArgs)
    {
        _lastFilterChangesEventArgs = lastFilterChangesEventArgs;

        await FinanceOperationsList.RefreshFinanceOperations(
            Search.SearchKeyword,
            _lastFilterChangesEventArgs.MinPrice,
            _lastFilterChangesEventArgs.MaxPrice,
            _lastFilterChangesEventArgs.DateStart,
            _lastFilterChangesEventArgs.DateEnd);
    }

    private async void OnSearchTextChanged(object sender, string searchKeyword)
    {
        await FinanceOperationsList.RefreshFinanceOperations(
            searchKeyword,
            _lastFilterChangesEventArgs?.MinPrice,
            _lastFilterChangesEventArgs?.MaxPrice,
            _lastFilterChangesEventArgs?.DateStart,
            _lastFilterChangesEventArgs?.DateEnd);
    }

    private async void GoBack(object obj)
    {
        await _eventAggregator.PublishOnUIThreadAsync(NavigationDirection.BudgetPlanSelectionPage);
    }

    private async void GoNext(object obj)
    {
        await _eventAggregator.PublishOnUIThreadAsync(NavigationDirection.BudgetPlanningPage);
    }
}