using System;
using System.ComponentModel;
using System.Globalization;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Wpf.Framework.Extensions;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

namespace Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Controls;

internal class FinanceOperationViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly IBudgetPlanContext _budgetPlanContext;

    private bool _editing;
    private Guid _financeOperationId;
    private DateTime _occurredAt;
    private string _price;
    private string _title;
    private bool _validationEnabled;

    private FinanceOperationViewModel(
        FinanceOperationDto financeOperation,
        IBudgetPlanContext budgetPlanContext)
    {
        _budgetPlanContext = budgetPlanContext;
        BudgetPlanId = financeOperation.BudgetPlanId;
        FinanceOperationId = financeOperation.Id;
        Title = financeOperation.Title;
        Price = financeOperation.Price.ToString(CultureInfo.InvariantCulture);
        Currency = financeOperation.Currency;
        OccurredAt = financeOperation.OccurredAt;
    }

    public FinanceOperationViewModel(IBudgetPlanContext budgetPlanContext)
    {
        _budgetPlanContext = budgetPlanContext;

        Editing = true;
        BudgetPlanId = budgetPlanContext.BudgetPlanId;
        Currency = budgetPlanContext.Currency;
        OccurredAt = DateTime.UtcNow;
    }

    public Guid BudgetPlanId { get; init; }
    public string Currency { get; init; }
    public ChangesMode ChangesMode { get; set; }

    public FinanceOperationType Type
    {
        get
        {
            var price = _price.ConvertToDecimalOrDefault();

            return price switch
            {
                0 => FinanceOperationType.None,
                > 0 => FinanceOperationType.Income,
                < 0 => FinanceOperationType.Expense
            };
        }
    }

    public bool Editing
    {
        get => _editing;
        set
        {
            _editing = value;

            OnPropertyChanged();
        }
    }

    public Guid FinanceOperationId
    {
        get => _financeOperationId;
        set
        {
            _financeOperationId = value;

            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value.ToUpperInvariant();

            if (!_validationEnabled)
                EnableValidation();

            OnPropertyChanged();
        }
    }

    public string Price
    {
        get => _price;
        set
        {
            _price = value;

            if (!_validationEnabled)
                EnableValidation();

            OnPropertyChanged();
            OnPropertyChanged(nameof(Type));
        }
    }

    public DateTime OccurredAt
    {
        get => _occurredAt;
        set
        {
            _occurredAt = value;

            if (!_validationEnabled)
                EnableValidation();

            OnPropertyChanged();
        }
    }

    public string Error { get; }

    public string this[string columnName]
    {
        get
        {
            if (!_validationEnabled)
                return string.Empty;

            var languageDictionary = ResourceDictionaryExtensions.GetLanguageDictionary();

            return columnName switch
            {
                nameof(Title) when !IsTitleValid() => languageDictionary.GetStringValue(
                    "OperationTitleValidationText"),
                nameof(Price) when !IsPriceValid() => languageDictionary.GetStringValue(
                    "OperationValueValidationText"),
                nameof(OccurredAt) when !IsDateStartValid() => languageDictionary.GetStringValue(
                    "OperationOccurredAtValidationText"),
                _ => string.Empty
            };
        }
    }

    public static FinanceOperationViewModel CreateFromDto(
        FinanceOperationDto financeOperationDto,
        IBudgetPlanContext budgetPlanContext)
    {
        return new FinanceOperationViewModel(financeOperationDto, budgetPlanContext);
    }

    public void EnableValidation()
    {
        _validationEnabled = true;
    }

    public void DisableValidation()
    {
        _validationEnabled = false;
    }

    public bool IsFormFilled()
    {
        return IsTitleValid() &&
               IsPriceValid() &&
               IsDateStartValid();
    }

    public override void Dispose()
    {
        Editing = false;
        FinanceOperationId = Guid.Empty;
        Title = null;
        Price = string.Empty;
        OccurredAt = default;
    }

    private bool IsTitleValid()
    {
        return !string.IsNullOrEmpty(Title) && Title.Length is < 60 and > 3;
    }

    private bool IsPriceValid()
    {
        var price = _price.ConvertToDecimalOrDefault();

        return price is > -1000000 and < 1000000 && price != 0;
    }

    private bool IsDateStartValid()
    {
        return OccurredAt != default &&
               OccurredAt > _budgetPlanContext.ValidFrom &&
               OccurredAt < _budgetPlanContext.ValidTo;
    }
}