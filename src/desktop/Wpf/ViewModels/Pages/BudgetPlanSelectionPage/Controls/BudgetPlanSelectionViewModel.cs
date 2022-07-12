using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.Settings;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Events;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.Mappings;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.StateMachines;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;

internal class BudgetPlanSelectionViewModel : ViewModelBase
{
    private readonly IBudgetPlanController _budgetPlanController;
    private readonly IBudgetPlanMapper _budgetPlanMapper;
    private readonly ISettingsProvider _settingsProvider;
    private IEnumerable<string> _availableCurrencies;
    private ObservableCollection<BudgetPlanViewModel> _budgetPlans;
    private BudgetPlanViewModelState _budgetPlanViewModelState;

    private bool _isBudgetPlanDetailsVisible;
    private BudgetPlanViewModel _previouslySelectedBudgetPlan;
    private BudgetPlanViewModel _selectedBudgetPlan;

    public BudgetPlanSelectionViewModel(
        IBudgetPlanController budgetPlanController,
        ISettingsProvider settingsProvider,
        IBudgetPlanMapper budgetPlanMapper)
    {
        _budgetPlanController = budgetPlanController;
        _settingsProvider = settingsProvider;
        _budgetPlanMapper = budgetPlanMapper;

        GoModifyCommand = new RelayCommand(GoModify, CanGoModify);
        GoCreateCommand = new RelayCommand(GoCreate);
    }

    public ICommand GoModifyCommand { get; }
    public ICommand GoCreateCommand { get; }

    public bool IsBudgetPlanDetailsVisible
    {
        get => _isBudgetPlanDetailsVisible;
        set
        {
            _isBudgetPlanDetailsVisible = value;

            OnPropertyChanged();
        }
    }

    public BudgetPlanViewModel SelectedBudgetPlan
    {
        get => _selectedBudgetPlan;
        set
        {
            _selectedBudgetPlan = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(CanStartOrModify));
        }
    }

    public ObservableCollection<BudgetPlanViewModel> BudgetPlans
    {
        get => _budgetPlans;
        set
        {
            _budgetPlans = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(HasAnyBudgetPlans));
        }
    }

    public bool HasAnyBudgetPlans => BudgetPlans?.Any() ?? false;
    public bool CanStartOrModify => PlanSelected();

    public override void Dispose()
    {
        _availableCurrencies = null;
        _budgetPlanViewModelState = null;
        _previouslySelectedBudgetPlan = null;

        IsBudgetPlanDetailsVisible = false;
        SelectedBudgetPlan = null;

        if (BudgetPlans.Any())
            foreach (var budgetPlan in BudgetPlans)
            {
                budgetPlan.Dispose();
                budgetPlan.GoBack -= GoBackEventHandler;
            }

        BudgetPlans = null;
    }

    public void Initialize(
        IEnumerable<BudgetPlanDto> budgetPlans,
        IEnumerable<string> availableCurrencies)
    {
        _availableCurrencies = availableCurrencies;
        var budgetPlansAsList = budgetPlans.ToList();

        if (budgetPlansAsList.Any())
        {
            var budgetPlanViewModels = _budgetPlanMapper
                .MapBudgetPlanDtosToBudgetPlanViewModels(
                    budgetPlansAsList,
                    _budgetPlanController,
                    _settingsProvider,
                    _availableCurrencies,
                    GoBackEventHandler);

            BudgetPlans = new ObservableCollection<BudgetPlanViewModel>(budgetPlanViewModels);
        }
        else
        {
            BudgetPlans = new ObservableCollection<BudgetPlanViewModel>();

            GoCreate(null);
        }
    }

    private void GoCreate(object obj)
    {
        ShowBudgetPlanDetails();

        var newBudgetPlan = new BudgetPlanViewModel(
            _budgetPlanController,
            _settingsProvider,
            _availableCurrencies);

        _previouslySelectedBudgetPlan = SelectedBudgetPlan;

        SelectedBudgetPlan = newBudgetPlan;
        SelectedBudgetPlan.BudgetPlanChangesMode = BudgetPlanChangesMode.Create;
        SelectedBudgetPlan.GoBack += GoBackEventHandler;

        if (BudgetPlans.Any())
            SelectedBudgetPlan.EnableValidation();
    }

    private bool CanGoModify(object obj)
    {
        return PlanSelected();
    }

    private void GoModify(object obj)
    {
        SelectedBudgetPlan.BudgetPlanChangesMode = BudgetPlanChangesMode.Modify;
        SelectedBudgetPlan.EnableValidation();
        _budgetPlanViewModelState = SelectedBudgetPlan.StoreCurrentState();

        ShowBudgetPlanDetails();
    }

    private bool PlanSelected()
    {
        return SelectedBudgetPlan != null;
    }

    private void ShowBudgetPlanDetails()
    {
        IsBudgetPlanDetailsVisible = true;
    }

    private void HideBudgetPlanDetails()
    {
        IsBudgetPlanDetailsVisible = false;
    }

    private void GoBackEventHandler(object sender, GoBackEventArgs e)
    {
        HideBudgetPlanDetails();

        var budgetPlanStateCoordinator = new BudgetPlanOperationStateCoordinator(
            GoBackEventHandler,
            _previouslySelectedBudgetPlan,
            _selectedBudgetPlan,
            _budgetPlanViewModelState);

        var currentState = budgetPlanStateCoordinator
            .GetCurrentState(
                e.BudgetPlanChangesMode,
                e.Cancelled);

        currentState.Process(this);

        OnPropertyChanged(nameof(SelectedBudgetPlan));
        OnPropertyChanged(nameof(HasAnyBudgetPlans));
    }
}