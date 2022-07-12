using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Events;
using Budgetter.Wpf.Framework.Extensions;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Controls;

internal class BudgetPlanningPageTargetViewModel : ViewModelBase
{
    private readonly IBudgetPlanContext _budgetPlanContext;
    private readonly IEventAggregator _eventAggregator;
    private readonly ITargetsController _targetsController;
    private readonly IFinanceOperationsController _financeOperationsController;

    private string _description;
    private decimal? _maxAmount;
    private BudgetPlanningPageTargetItemViewModel _selectedTargetItem;
    private ObservableCollection<BudgetPlanningPageTargetItemViewModel> _targetItems;
    private string _title;
    private DateTime _validFrom;
    private DateTime _validTo;

    public BudgetPlanningPageTargetViewModel(
        IEventAggregator eventAggregator,
        ITargetsController targetsController,
        IBudgetPlanContext budgetPlanContext,
        IFinanceOperationsController financeOperationsController)
    {
        _eventAggregator = eventAggregator;
        _targetsController = targetsController;
        _budgetPlanContext = budgetPlanContext;
        _financeOperationsController = financeOperationsController;

        ValidFrom = DateTime.UtcNow;
        ValidTo = DateTime.UtcNow;

        TargetItems = new ObservableCollection<BudgetPlanningPageTargetItemViewModel>();
        AllocateExpensesCommand = new RelayCommand(AllocateExpenses);
    }

    public ICommand AllocateExpensesCommand { get; }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;

            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;

            OnPropertyChanged();
        }
    }

    public decimal? MaxAmount
    {
        get => _maxAmount;
        set
        {
            _maxAmount = value;

            OnPropertyChanged();
        }
    }

    public DateTime ValidFrom
    {
        get => _validFrom;
        set
        {
            _validFrom = value;

            OnPropertyChanged();
        }
    }

    public DateTime ValidTo
    {
        get => _validTo;
        set
        {
            _validTo = value;

            OnPropertyChanged();
        }
    }

    public BudgetPlanningPageTargetItemViewModel SelectedTargetItem
    {
        get => _selectedTargetItem;
        set
        {
            _selectedTargetItem = value;

            OnPropertyChanged();
        }
    }

    public ObservableCollection<BudgetPlanningPageTargetItemViewModel> TargetItems
    {
        get => _targetItems;
        set
        {
            _targetItems = value;

            OnPropertyChanged();
        }
    }

    public Guid Id { get; init; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; init; }
    public TargetStatus Status { get; set; }
    public TargetBalance Balance { get; set; }

    private async void AllocateExpenses(object obj)
    {
        if (obj is not IList<BudgetPlanningPageExpenseViewModel> expenses) return;

        var expensesAsList = expenses.ToList();

        foreach (var expense in expensesAsList)
        {
            var targetItem = new BudgetPlanningPageTargetItemViewModel
            {
                Id = expense.ExpenseId,
                TargetId = Id,
                Title = expense.Title,
                UnitPrice = expense.Price
                    .ConvertToDecimalOrDefault(),
                Currency = expense.Currency,
                OccurredAt = expense.OccurredAt
            };

            TargetItems.Add(targetItem);

            await _targetsController.AllocateExpenseAsync(
                Id,
                new FinanceOperationDto
                {
                    Id = expense.ExpenseId,
                    BudgetPlanId = _budgetPlanContext.BudgetPlanId,
                    Title = expense.Title,
                    Price = expense.Price
                        .ConvertToDecimalOrDefault(),
                    Currency = expense.Currency,
                    Type = FinanceOperationTypeDto.Expense,
                    OccurredAt = expense.OccurredAt
                });

            await _financeOperationsController.ChangeFinanceOperationAssignationStatus(
                expense.ExpenseId,
                Id,
                true);
        }

        await _eventAggregator.PublishOnUIThreadAsync(
            new ExpensesAllocatedEvent(expensesAsList));
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public void Clear()
    {
        Title = default;
        Description = default;
        MaxAmount = default;
        ValidFrom = DateTime.UtcNow;
        ValidTo = DateTime.UtcNow;
    }
}