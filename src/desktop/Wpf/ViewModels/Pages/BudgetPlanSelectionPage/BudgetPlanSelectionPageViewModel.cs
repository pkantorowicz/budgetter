using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Events;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.Settings;
using Budgetter.Wpf.ViewModels.Helpers;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.Mappings;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage;

internal class BudgetPlanSelectionPageViewModel : ViewModelBase, IPageViewModel
{
    private readonly IBudgetPlanContext _budgetPlanContext;
    private readonly IBudgetPlanController _budgetPlanController;
    private readonly IBudgetPlanMapper _budgetPlanMapper;
    private readonly IEventAggregator _eventAggregator;
    private readonly IEchangeRatesController _exchangeRatesController;
    private readonly ISettingsProvider _settingsProvider;

    private BudgetPlanSelectionViewModel _budgetPlanSelectionViewModel;

    public BudgetPlanSelectionPageViewModel(
        IEventAggregator eventAggregator,
        IBudgetPlanController budgetPlanController,
        IEchangeRatesController exchangeRatesController,
        ISettingsProvider settingsProvider,
        IBudgetPlanMapper budgetPlanMapper,
        IBudgetPlanContext budgetPlanContext)
    {
        _eventAggregator = eventAggregator;
        _budgetPlanController = budgetPlanController;
        _exchangeRatesController = exchangeRatesController;
        _settingsProvider = settingsProvider;
        _budgetPlanMapper = budgetPlanMapper;
        _budgetPlanContext = budgetPlanContext;

        StartCommand = new RelayCommand(Start, CanStart);
    }

    public ICommand StartCommand { get; }

    public BudgetPlanSelectionViewModel BudgetPlanSelectionViewModel
    {
        get => _budgetPlanSelectionViewModel;
        set
        {
            _budgetPlanSelectionViewModel = value;

            OnPropertyChanged();
        }
    }

    public async Task OnDeactivateAsync()
    {
        Dispose();

        await Task.CompletedTask;
    }

    public async Task OnActivateAsync()
    {
        var budgetPlans = await _budgetPlanController.GetBudgetPlans();
        var availableCurrencies = await _exchangeRatesController.GetAvailableCurrencies();

        BudgetPlanSelectionViewModel =
            new BudgetPlanSelectionViewModel(
                _budgetPlanController,
                _settingsProvider,
                _budgetPlanMapper);

        BudgetPlanSelectionViewModel.Initialize(
            budgetPlans,
            availableCurrencies);
    }

    public override void Dispose()
    {
        if (_budgetPlanSelectionViewModel is null)
            return;

        _budgetPlanSelectionViewModel.Dispose();
        BudgetPlanSelectionViewModel = null;
    }

    private bool CanStart(object obj)
    {
        return _budgetPlanSelectionViewModel.CanStartOrModify;
    }

    private async void Start(object obj)
    {
        var selectedBudgetPlan = _budgetPlanSelectionViewModel.SelectedBudgetPlan;

        if (selectedBudgetPlan is null)
            throw new ArgumentException(
                "Unable to create budget context, because budget plan is not selected.");

        _budgetPlanContext.ModifyState(
            selectedBudgetPlan.BudgetPlanId,
            selectedBudgetPlan.Title,
            selectedBudgetPlan.Currency,
            selectedBudgetPlan.ValidFrom,
            selectedBudgetPlan.ValidTo);

        await _eventAggregator.PublishOnUIThreadAsync(NavigationDirection.FinanceOperationsPage);
    }
}