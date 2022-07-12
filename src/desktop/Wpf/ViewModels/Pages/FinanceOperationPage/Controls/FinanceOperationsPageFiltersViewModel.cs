using System;
using System.Windows.Input;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Controls;

internal class FinanceOperationsPageFiltersViewModel : ViewModelBase
{
    private readonly FilterChangesHelper _filtersChangesHelper;
    private bool _currentMonth;
    private bool _currentMonthEnabled;
    private DateTime? _endDate;
    private bool _lastHalfYear;
    private bool _lastHalfYearEnabled;
    private bool _lastMonth;
    private bool _lastMonthEnabled;
    private bool _lastThreeMonths;
    private bool _lastThreeMonthsEnabled;
    private string _maxPrice;
    private string _minPrice;

    private DateTime? _startDate;

    public FinanceOperationsPageFiltersViewModel(IBudgetPlanContext budgetPlanContext)
    {
        _filtersChangesHelper = new FilterChangesHelper();

        ClearCommand = new RelayCommand(Clear, CanClear);

        PredicateRadioButtonState(budgetPlanContext);
    }

    public ICommand ClearCommand { get; }

    public DateTime? StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;

            _filtersChangesHelper.UpdateFiltersState(
                FilterChangeType.StartDate,
                value);

            OnDateSelected();
            OnFiltersChanged();
            OnPropertyChanged();
        }
    }

    public DateTime? EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;

            _filtersChangesHelper.UpdateFiltersState(
                FilterChangeType.EndDate,
                value);

            OnDateSelected();
            OnFiltersChanged();
            OnPropertyChanged();
        }
    }

    public string MinPrice
    {
        get => _minPrice;
        set
        {
            decimal minPrice = 0;

            if (!string.IsNullOrEmpty(value) && !decimal.TryParse(value, out minPrice))
                return;

            _minPrice = minPrice != 0 ? minPrice.ToString("N") : string.Empty;

            _filtersChangesHelper.UpdateFiltersState(
                FilterChangeType.MinPrice,
                value);

            OnFiltersChanged();
            OnPropertyChanged();
        }
    }

    public string MaxPrice
    {
        get => _maxPrice;
        set
        {
            decimal maxPrice = 0;

            if (!string.IsNullOrEmpty(value) && !decimal.TryParse(value, out maxPrice))
                return;

            _maxPrice = maxPrice != 0 ? maxPrice.ToString("N") : string.Empty;

            _filtersChangesHelper.UpdateFiltersState(
                FilterChangeType.MaxPrice,
                value);

            OnFiltersChanged();
            OnPropertyChanged();
        }
    }

    public bool CurrentMonth
    {
        get => _currentMonth;
        set
        {
            _currentMonth = value;

            _filtersChangesHelper.UpdateFiltersState(
                FilterChangeType.CurrentMonth,
                value);

            OnDatePickersCleared();
            OnFiltersChanged();
            OnPropertyChanged();
        }
    }

    public bool LastMonth
    {
        get => _lastMonth;
        set
        {
            _lastMonth = value;

            _filtersChangesHelper.UpdateFiltersState(
                FilterChangeType.LastMonth,
                value);

            OnDatePickersCleared();
            OnFiltersChanged();
            OnPropertyChanged();
        }
    }

    public bool LastThreeMonths
    {
        get => _lastThreeMonths;
        set
        {
            _lastThreeMonths = value;

            _filtersChangesHelper.UpdateFiltersState(
                FilterChangeType.LastThreeMonths,
                value);

            OnDatePickersCleared();
            OnFiltersChanged();
            OnPropertyChanged();
        }
    }

    public bool LastHalfYear
    {
        get => _lastHalfYear;
        set
        {
            _lastHalfYear = value;

            _filtersChangesHelper.UpdateFiltersState(
                FilterChangeType.LastHalfYear,
                value);

            OnDatePickersCleared();
            OnFiltersChanged();
            OnPropertyChanged();
        }
    }

    public bool CurrentMonthEnabled
    {
        get => _currentMonthEnabled;
        set
        {
            _currentMonthEnabled = value;

            OnPropertyChanged();
        }
    }

    public bool LastMonthEnabled
    {
        get => _lastMonthEnabled;
        set
        {
            _lastMonthEnabled = value;

            OnPropertyChanged();
        }
    }

    public bool LastThreeMonthsEnabled
    {
        get => _lastThreeMonthsEnabled;
        set
        {
            _lastThreeMonthsEnabled = value;

            OnPropertyChanged();
        }
    }

    public bool LastHalfYearEnabled
    {
        get => _lastHalfYearEnabled;
        set
        {
            _lastHalfYearEnabled = value;

            OnPropertyChanged();
        }
    }

    public event EventHandler<FilterChangesEventArgs> FiltersChanged;

    protected virtual void OnFiltersChanged()
    {
        var filterChanges = _filtersChangesHelper.GetChanges();
        var filterChangesEventArgs = new FilterChangesEventArgs();
        var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        foreach (var filterChange in filterChanges)
            switch (filterChange.Key)
            {
                case FilterChangeType.None:
                    break;
                case FilterChangeType.StartDate:
                    filterChangesEventArgs.DateStart = (DateTime?)filterChange.Value;
                    break;
                case FilterChangeType.EndDate:
                    filterChangesEventArgs.DateEnd = (DateTime?)filterChange.Value;
                    break;
                case FilterChangeType.MinPrice:
                    decimal.TryParse(filterChange.Value.ToString(), out var minPrice);
                    filterChangesEventArgs.MinPrice = minPrice == 0 ? null : minPrice;
                    break;
                case FilterChangeType.MaxPrice:
                    decimal.TryParse(filterChange.Value.ToString(), out var maxPrice);
                    filterChangesEventArgs.MaxPrice = maxPrice == 0 ? null : maxPrice;
                    break;
                case FilterChangeType.CurrentMonth:
                    if (filterChange.Value is false) break;
                    filterChangesEventArgs.DateStart = month;
                    filterChangesEventArgs.DateEnd = month.AddMonths(1).AddDays(-1);
                    break;
                case FilterChangeType.LastMonth:
                    if (filterChange.Value is false) break;
                    filterChangesEventArgs.DateStart = month.AddMonths(-1);
                    filterChangesEventArgs.DateEnd = month.AddDays(-1);
                    break;
                case FilterChangeType.LastThreeMonths:
                    if (filterChange.Value is false) break;
                    filterChangesEventArgs.DateStart = month.AddMonths(-3);
                    filterChangesEventArgs.DateEnd = month.AddDays(-1);
                    break;
                case FilterChangeType.LastHalfYear:
                    if (filterChange.Value is false) break;
                    filterChangesEventArgs.DateStart = month.AddMonths(-6);
                    filterChangesEventArgs.DateEnd = month.AddDays(-1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterChange.Key), filterChange.Key,
                        "Unknown filter change type.");
            }

        FiltersChanged?.Invoke(this, filterChangesEventArgs);
    }

    private void OnDateSelected()
    {
        _currentMonth = false;
        _lastMonth = false;
        _lastThreeMonths = false;
        _lastHalfYear = false;

        OnPropertyChanged(nameof(CurrentMonth));
        OnPropertyChanged(nameof(LastMonth));
        OnPropertyChanged(nameof(LastThreeMonths));
        OnPropertyChanged(nameof(LastHalfYear));

        _filtersChangesHelper.RemoveRadioButtonsChanges();
    }

    private void OnDatePickersCleared()
    {
        _startDate = default;
        _endDate = default;

        OnPropertyChanged(nameof(StartDate));
        OnPropertyChanged(nameof(EndDate));

        _filtersChangesHelper.RemoveDatePickerChanges();
    }

    private void PredicateRadioButtonState(IBudgetPlanContext budgetPlanContext)
    {
        var budgetPlanDuration = DateTime.Today - budgetPlanContext.ValidFrom;
        var budgetPlanDurationDays = budgetPlanDuration?.Days > 0 ? budgetPlanDuration.Value.Days : 0;

        if (budgetPlanDurationDays > 180)
        {
            CurrentMonthEnabled = true;
            LastMonthEnabled = true;
            LastThreeMonthsEnabled = true;
            LastHalfYearEnabled = true;
        }

        if (budgetPlanDurationDays > 90)
        {
            CurrentMonthEnabled = true;
            LastMonthEnabled = true;
            LastThreeMonthsEnabled = true;
        }

        if (budgetPlanDurationDays > 30)
        {
            CurrentMonthEnabled = true;
            LastMonthEnabled = true;
        }

        if (budgetPlanDurationDays > 0) CurrentMonthEnabled = true;
    }

    private bool CanClear(object arg)
    {
        var canClear = _startDate != default ||
                       _endDate != default ||
                       !string.IsNullOrEmpty(_minPrice) ||
                       !string.IsNullOrEmpty(_maxPrice) ||
                       _currentMonth != default ||
                       _lastMonth != default ||
                       _lastThreeMonths != default ||
                       _lastHalfYear != default;

        return canClear;
    }

    private void Clear(object obj)
    {
        StartDate = default;
        EndDate = default;
        MinPrice = default;
        MaxPrice = default;
        CurrentMonth = default;
        LastMonth = default;
        LastThreeMonths = default;
        LastHalfYear = default;

        _filtersChangesHelper.Clear();
    }
}