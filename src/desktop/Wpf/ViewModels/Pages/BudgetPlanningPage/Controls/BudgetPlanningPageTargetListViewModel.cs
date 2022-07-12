using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Events;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Controls;

internal class BudgetPlanningPageTargetListViewModel : ViewModelBase
{
    private readonly IBudgetPlanContext _budgetPlanContext;
    private readonly IEventAggregator _eventAggregator;
    private readonly ITargetsController _targetsController;
    private readonly IFinanceOperationsController _financeOperationsController;

    private bool _isDialogOpen;
    private BudgetPlanningPageTargetViewModel _selectedTarget;
    private IList<BudgetPlanningPageTargetViewModel> _targets;

    public BudgetPlanningPageTargetListViewModel(
        IBudgetPlanContext budgetPlanContext,
        ITargetsController targetsController,
        IEventAggregator eventAggregator,
        IFinanceOperationsController financeOperationsController)
    {
        _budgetPlanContext = budgetPlanContext;
        _targetsController = targetsController;
        _eventAggregator = eventAggregator;
        _financeOperationsController = financeOperationsController;

        AddCommand = new RelayCommand(Add);
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    public ICommand AddCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public bool IsDialogOpen
    {
        get => _isDialogOpen;
        set
        {
            _isDialogOpen = value;

            OnPropertyChanged();
        }
    }

    public IList<BudgetPlanningPageTargetViewModel> Targets
    {
        get => _targets;
        set
        {
            _targets = value;

            OnPropertyChanged();
        }
    }

    public BudgetPlanningPageTargetViewModel SelectedTarget
    {
        get => _selectedTarget;
        set
        {
            _selectedTarget = value;

            OnPropertyChanged();
        }
    }

    public async Task GetTargets()
    {
        var targets = await _targetsController.GetTargets(
            _budgetPlanContext.BudgetPlanId,
            _budgetPlanContext.ValidFrom.GetValueOrDefault(),
            _budgetPlanContext.ValidTo.GetValueOrDefault());

        Targets = new ObservableCollection<BudgetPlanningPageTargetViewModel>(targets
            .Select(t =>
                new BudgetPlanningPageTargetViewModel(
                    _eventAggregator,
                    _targetsController,
                    _budgetPlanContext,
                    _financeOperationsController)
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    MaxAmount = t.MaxAmount,
                    ValidFrom = t.ValidFrom,
                    ValidTo = t.ValidTo,
                    TotalAmount = t.TotalAmount,
                    Currency = t.Currency,
                    Status = (TargetStatus)t.Status,
                    Balance = (TargetBalance)t.Balance,
                    TargetItems = new ObservableCollection<BudgetPlanningPageTargetItemViewModel>(t.TargetItems
                        .Select(ti => new BudgetPlanningPageTargetItemViewModel
                        {
                            Id = ti.Id,
                            TargetId = ti.TargetId,
                            Title = ti.Title,
                            UnitPrice = ti.UnitPrice,
                            Currency = ti.Currency,
                            OccurredAt = ti.OccurredAt
                        }).ToList())
                })
            .ToList());
    }

    private void Add(object obj)
    {
        IsDialogOpen = true;

        SelectedTarget = new BudgetPlanningPageTargetViewModel(
            _eventAggregator,
            _targetsController,
            _budgetPlanContext,
            _financeOperationsController);
    }

    private void Cancel(object obj)
    {
        SelectedTarget?.Clear();
        SelectedTarget = null;
        IsDialogOpen = false;
    }

    private async void Save(object obj)
    {
        var targetId = await _targetsController.CreateTargetAsync(
            _budgetPlanContext.BudgetPlanId,
            SelectedTarget.Title,
            SelectedTarget.Description,
            SelectedTarget.ValidFrom,
            SelectedTarget.ValidTo,
            _budgetPlanContext.ValidFrom.GetValueOrDefault(),
            _budgetPlanContext.ValidTo.GetValueOrDefault(),
            SelectedTarget.MaxAmount.GetValueOrDefault(),
            _budgetPlanContext.Currency);

        Targets.Add(SelectedTarget);
        SelectedTarget = null;
        IsDialogOpen = false;
    }
}