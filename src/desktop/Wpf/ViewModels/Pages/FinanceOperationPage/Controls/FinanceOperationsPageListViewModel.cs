using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Extensions;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Controls;

internal class FinanceOperationsPageListViewModel : ViewModelBase
{
    private readonly IBudgetPlanContext _budgetPlanContext;
    private readonly IFinanceOperationsController _financeOperationsController;

    private FinanceOperationChanges _financeOperationChanges;
    private ObservableCollection<FinanceOperationViewModel> _financeOperations;
    private bool _isAddingInProgress;
    private FinanceOperationViewModel _selectedFinanceOperation;

    public FinanceOperationsPageListViewModel(
        IBudgetPlanContext budgetPlanContext,
        IFinanceOperationsController financeOperationsController)
    {
        _budgetPlanContext = budgetPlanContext;
        _financeOperationsController = financeOperationsController;

        AddCommand = new RelayCommand(AddFinanceOperation);
        EditCommand = new RelayCommand(EditFinanceOperation);
        RemoveCommand = new RelayCommand(RemoveFinanceOperation);
        ConfirmCommand = new RelayCommand(ConfirmChanges, CanConfirmChanges);
        CancelCommand = new RelayCommand(RejectChanges);
    }

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    public decimal IncomesSummary => FinanceOperations
        .Where(fo => fo.Type == FinanceOperationType.Income)
        .Sum(fo => fo.Price.ConvertToDecimalOrDefault());

    public decimal ExpensesSummary => FinanceOperations
        .Where(fo => fo.Type == FinanceOperationType.Expense)
        .Sum(fo => fo.Price.ConvertToDecimalOrDefault());

    public FinanceOperationViewModel SelectedFinanceOperation
    {
        get => _selectedFinanceOperation;
        set
        {
            _selectedFinanceOperation = value;

            OnPropertyChanged();
        }
    }

    public ObservableCollection<FinanceOperationViewModel> FinanceOperations
    {
        get => _financeOperations;
        set
        {
            _financeOperations = value;

            OnPropertyChanged();
            OnFinanceOperationsListUpdated();
        }
    }

    public async Task RefreshFinanceOperations(
        string titleKeyword = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        DateTime? dateStart = null,
        DateTime? dateEnd = null,
        bool? onlyAssigned = null)
    {
        var financeOperations =
            await _financeOperationsController.FilterFinanceOperationsAsync(
                _budgetPlanContext.BudgetPlanId,
                titleKeyword,
                minPrice,
                maxPrice,
                dateStart,
                dateEnd,
                onlyAssigned);

        FinanceOperations = new ObservableCollection<FinanceOperationViewModel>(
            financeOperations
                .Select(fo => FinanceOperationViewModel.CreateFromDto(fo, _budgetPlanContext))
                .OrderByDescending(fo => fo.OccurredAt)
                .ToList());
    }

    private void AddFinanceOperation(object obj)
    {
        if (_isAddingInProgress)
            return;

        var newFinanceOperation = new FinanceOperationViewModel(_budgetPlanContext);

        FinanceOperations.Insert(0, newFinanceOperation);
        SelectedFinanceOperation = newFinanceOperation;
        SelectedFinanceOperation.DisableValidation();
        SelectedFinanceOperation.ChangesMode = ChangesMode.Create;
        _isAddingInProgress = true;
    }

    private void EditFinanceOperation(object obj)
    {
        SelectedFinanceOperation = (FinanceOperationViewModel)obj;
        SelectedFinanceOperation.Editing = true;
        SelectedFinanceOperation.ChangesMode = ChangesMode.Edit;

        _financeOperationChanges = new FinanceOperationChanges(
            SelectedFinanceOperation.Title,
            SelectedFinanceOperation.Price.ConvertToDecimalOrDefault(),
            SelectedFinanceOperation.OccurredAt);
    }

    private void RemoveFinanceOperation(object obj)
    {
        // todo implementation for removing finance operations is necessary
    }

    private bool CanConfirmChanges(object arg)
    {
        return SelectedFinanceOperation?.IsFormFilled() == true;
    }

    private async void ConfirmChanges(object obj)
    {
        SelectedFinanceOperation = (FinanceOperationViewModel)obj;

        switch (SelectedFinanceOperation.ChangesMode)
        {
            case ChangesMode.Create:
                SelectedFinanceOperation.FinanceOperationId =
                    await _financeOperationsController.CreateFinanceOperation(
                        _budgetPlanContext.BudgetPlanId,
                        SelectedFinanceOperation.Title,
                        SelectedFinanceOperation.Price.ConvertToDecimalOrDefault(),
                        SelectedFinanceOperation.Currency,
                        _budgetPlanContext.Currency,
                        _budgetPlanContext.ValidFrom,
                        _budgetPlanContext.ValidTo,
                        SelectedFinanceOperation.OccurredAt);

                _isAddingInProgress = false;
                break;
            case ChangesMode.Edit:
                await _financeOperationsController.ChangeFinanceOperationAttributes(
                    SelectedFinanceOperation.FinanceOperationId,
                    SelectedFinanceOperation.Title,
                    SelectedFinanceOperation.Price.ConvertToDecimalOrDefault());
                break;
            case ChangesMode.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(SelectedFinanceOperation.ChangesMode),
                    SelectedFinanceOperation?.ChangesMode, "Unknown changes mode.");
        }

        SelectedFinanceOperation.ChangesMode = ChangesMode.None;
        SelectedFinanceOperation.Editing = false;

        SortFinanceOperations();
    }

    private void RejectChanges(object obj)
    {
        SelectedFinanceOperation = (FinanceOperationViewModel)obj;
        SelectedFinanceOperation.Editing = false;

        switch (SelectedFinanceOperation.ChangesMode)
        {
            case ChangesMode.Create:
                FinanceOperations.Remove(SelectedFinanceOperation);
                _isAddingInProgress = false;
                break;
            case ChangesMode.Edit:
                SelectedFinanceOperation.Title = _financeOperationChanges.Title;
                SelectedFinanceOperation.Price =
                    _financeOperationChanges.Price.ToString(CultureInfo.InvariantCulture);
                SelectedFinanceOperation.OccurredAt = _financeOperationChanges.OccurredAt;
                SelectedFinanceOperation.ChangesMode = ChangesMode.None;
                break;
            case ChangesMode.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(SelectedFinanceOperation.ChangesMode),
                    SelectedFinanceOperation?.ChangesMode, "Unknown changes mode.");
        }
    }

    private void OnFinanceOperationsListUpdated()
    {
        OnPropertyChanged(nameof(IncomesSummary));
        OnPropertyChanged(nameof(ExpensesSummary));
    }
 
    private void SortFinanceOperations()
    {
        FinanceOperations = new ObservableCollection<FinanceOperationViewModel>(_financeOperations
            .OrderByDescending(fo => fo.OccurredAt)
            .ToList());
    }
}