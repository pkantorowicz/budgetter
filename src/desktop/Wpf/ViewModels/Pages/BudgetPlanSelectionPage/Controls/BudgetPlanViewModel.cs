using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Extensions;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.Settings;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Events;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;

internal class BudgetPlanViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly IBudgetPlanController _budgetPlanController;
    private readonly ISettingsProvider _settingsProvider;

    private ObservableCollection<string> _availableCurrencies;
    private BudgetPlanChangesMode _budgetPlanChangesMode;
    private Guid _budgetPlanId;
    private string _currency;
    private bool _isMonthCheckBoxChecked;
    private bool _isQuarterCheckBoxChecked;
    private string _title;
    private bool _validationEnabled;
    private DateTime? _validFrom;
    private DateTime? _validTo;

    public BudgetPlanViewModel(
        IBudgetPlanController budgetPlanController,
        ISettingsProvider settingsProvider,
        IEnumerable<string> availableCurrencies)
    {
        _budgetPlanController = budgetPlanController;
        _settingsProvider = settingsProvider;

        AvailableCurrencies = new ObservableCollection<string>(availableCurrencies);

        CreateCommand = new RelayCommand(Create, CanCreate);
        ModifyCommand = new RelayCommand(Modify, CanModify);
        CancelCommand = new RelayCommand(Cancel);

        DisableValidation();
    }

    public ICommand CreateCommand { get; }
    public ICommand ModifyCommand { get; }
    public ICommand CancelCommand { get; }

    public Guid BudgetPlanId
    {
        get => _budgetPlanId;
        set
        {
            _budgetPlanId = value;

            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;

            OnPropertyChanged();
        }
    }

    public string Currency
    {
        get => _currency;
        set
        {
            _currency = value;

            OnPropertyChanged();
        }
    }

    public DateTime? ValidFrom
    {
        get => _validFrom;
        set
        {
            _validFrom = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(IsMonthCheckBoxChecked));
            OnPropertyChanged(nameof(IsQuarterCheckBoxChecked));
        }
    }

    public DateTime? ValidTo
    {
        get => _validTo;
        set
        {
            _validTo = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(IsMonthCheckBoxChecked));
            OnPropertyChanged(nameof(IsQuarterCheckBoxChecked));
        }
    }

    public BudgetPlanChangesMode BudgetPlanChangesMode
    {
        get => _budgetPlanChangesMode;
        set
        {
            _budgetPlanChangesMode = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(CreateMode));
            OnPropertyChanged(nameof(EditMode));
        }
    }

    public bool IsMonthCheckBoxChecked
    {
        get => DateTime.UtcNow.CheckMonthDates(_validFrom, _validTo);
        set
        {
            _isMonthCheckBoxChecked = value;

            MonthlyCheckboxChecked(_isMonthCheckBoxChecked);

            OnPropertyChanged();
            OnPropertyChanged(nameof(ValidFrom));
            OnPropertyChanged(nameof(ValidTo));
        }
    }

    public bool IsQuarterCheckBoxChecked
    {
        get => DateTime.UtcNow.CheckQuarterDates(_validFrom, _validTo);
        set
        {
            _isQuarterCheckBoxChecked = value;

            QuarterlyCheckboxChecked(_isQuarterCheckBoxChecked);

            OnPropertyChanged();
            OnPropertyChanged(nameof(ValidFrom));
            OnPropertyChanged(nameof(ValidTo));
        }
    }

    public ObservableCollection<string> AvailableCurrencies
    {
        get => _availableCurrencies;
        set
        {
            _availableCurrencies = value;

            OnPropertyChanged();
        }
    }

    public bool CreateMode => BudgetPlanChangesMode == BudgetPlanChangesMode.Create;
    public bool EditMode => BudgetPlanChangesMode == BudgetPlanChangesMode.Modify;
    public string Error => null;

    public string this[string columnName]
    {
        get
        {
            if (!_validationEnabled)
                return string.Empty;

            var languageDictionary = ResourceDictionaryExtensions.GetLanguageDictionary();

            return columnName switch
            {
                nameof(Title) when !IsTitleValid() => languageDictionary.GetStringValue("TitleValidationText"),
                nameof(Currency) when !IsCurrencyValid() => languageDictionary.GetStringValue(
                    "CurrencyValidationText"),
                nameof(ValidFrom) when !IsDateStartValid() => languageDictionary.GetStringValue(
                    "ValidFromValidationText"),
                nameof(ValidTo) when !IsDateEndValid() =>
                    languageDictionary.GetStringValue("ValidToValidationText"),
                _ => string.Empty
            };
        }
    }

    public event EventHandler<GoBackEventArgs> GoBack;

    public BudgetPlanViewModelState StoreCurrentState()
    {
        return new BudgetPlanViewModelState(
            _title,
            _currency,
            _validFrom,
            _validTo);
    }

    public void EnableValidation()
    {
        _validationEnabled = true;
    }

    public void DisableValidation()
    {
        _validationEnabled = false;
    }

    public void RestoreChanges(BudgetPlanViewModelState budgetPlanViewModelState)
    {
        Title = budgetPlanViewModelState.Title;
        Currency = budgetPlanViewModelState.Currency;
        ValidFrom = budgetPlanViewModelState.ValidFrom;
        ValidTo = budgetPlanViewModelState.ValidTo;
    }

    private bool CanModify(object obj)
    {
        return _budgetPlanId != Guid.Empty && IsFormFilled();
    }

    private async void Modify(object obj)
    {
        await ModifyPlan();
        Back();
    }

    private bool CanCreate(object obj)
    {
        return IsFormFilled();
    }

    private async void Create(object obj)
    {
        await CreateNewPlan();
        Back();
    }

    private void Cancel(object obj)
    {
        Back(true);
    }

    private async Task CreateNewPlan()
    {
        BudgetPlanId = await _budgetPlanController.CreateBudgetPlan(
            _title,
            _currency,
            _validFrom.GetValueOrDefault(),
            _validTo.GetValueOrDefault(),
            _settingsProvider.GetNbpApiSettings()
                .MaxRecordCount);
    }

    private async Task ModifyPlan()
    {
        await _budgetPlanController.ChangeBudgetPlanAttributes(
            _budgetPlanId,
            _title,
            _currency,
            _validFrom.GetValueOrDefault(),
            _validTo.GetValueOrDefault(),
            _settingsProvider.GetNbpApiSettings()
                .MaxRecordCount);
    }

    private void Back(bool cancelled = false)
    {
        GoBack?.Invoke(this, new GoBackEventArgs(_budgetPlanChangesMode, cancelled));
    }

    private bool IsFormFilled()
    {
        return IsTitleValid() &&
               IsCurrencyValid() &&
               IsDateStartValid() &&
               IsDateEndValid();
    }

    private bool IsTitleValid()
    {
        return !string.IsNullOrEmpty(Title) && Title.Length is < 60 and > 3;
    }

    private bool IsCurrencyValid()
    {
        return !string.IsNullOrEmpty(Currency) && _availableCurrencies.Contains(Currency);
    }

    private bool IsDateStartValid()
    {
        return ValidFrom is not null && (ValidTo is null || ValidFrom < ValidTo &&
            (ValidTo - ValidFrom).GetValueOrDefault().Days < _settingsProvider.GetMaxSupportedDaysCount());
    }

    private bool IsDateEndValid()
    {
        return ValidTo is not null && (ValidFrom is null || ValidTo > ValidFrom &&
            (ValidTo - ValidFrom).GetValueOrDefault().Days < _settingsProvider.GetMaxSupportedDaysCount());
    }

    private void MonthlyCheckboxChecked(bool isChecked)
    {
        var (monthStartDate, monthEndDate) = DateTime.UtcNow
            .GetMonthStartAndEndDates();

        CheckboxChecked(
            isChecked,
            monthStartDate,
            monthEndDate);
    }

    private void QuarterlyCheckboxChecked(bool isChecked)
    {
        var (quarterStartDate, quarterEndDate) = DateTime.UtcNow
            .GetQuarterStartAndEndDates();

        CheckboxChecked(
            isChecked,
            quarterStartDate,
            quarterEndDate);
    }

    private void CheckboxChecked(
        bool isChecked,
        DateTime validFrom,
        DateTime validTo)
    {
        if (isChecked != true)
            return;

        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public override void Dispose()
    {
        _validationEnabled = false;

        BudgetPlanId = Guid.Empty;
        Title = null;
        Currency = null;
        ValidFrom = null;
        ValidTo = null;
        IsMonthCheckBoxChecked = false;
        IsQuarterCheckBoxChecked = false;
        AvailableCurrencies = null;
    }
}