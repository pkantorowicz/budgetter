using System;
using System.Globalization;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Wpf.Framework.ViewModels;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Controls;

internal class BudgetPlanningPageExpenseViewModel : ViewModelBase
{
    private Guid _expenseId;
    private DateTime _occurredAt;
    private string _price;
    private string _title;

    private BudgetPlanningPageExpenseViewModel(FinanceOperationDto financeOperation)
    {
        ExpenseId = financeOperation.Id;
        Title = financeOperation.Title;
        Price = financeOperation.Price.ToString(CultureInfo.InvariantCulture);
        Currency = financeOperation.Currency;
        OccurredAt = financeOperation.OccurredAt;
    }

    public string Currency { get; }

    public Guid ExpenseId
    {
        get => _expenseId;
        set
        {
            _expenseId = value;

            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value.ToUpperInvariant();

            OnPropertyChanged();
        }
    }

    public string Price
    {
        get => _price;
        set
        {
            _price = value;

            OnPropertyChanged();
        }
    }

    public DateTime OccurredAt
    {
        get => _occurredAt;
        set
        {
            _occurredAt = value;

            OnPropertyChanged();
        }
    }

    public static BudgetPlanningPageExpenseViewModel CreateFromDto(
        FinanceOperationDto financeOperationDto)
    {
        return new BudgetPlanningPageExpenseViewModel(financeOperationDto);
    }

    public override void Dispose()
    {
        ExpenseId = Guid.Empty;
        Title = null;
        Price = string.Empty;
        OccurredAt = default;
    }
}