using System;
using Budgetter.Wpf.Framework.ViewModels;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage.Controls;

internal class BudgetPlanningPageTargetItemViewModel : ViewModelBase
{
    private string _title;
    private decimal _unitPrice;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;

            OnPropertyChanged();
        }
    }

    public decimal UnitPrice
    {
        get => _unitPrice;
        set
        {
            _unitPrice = value;

            OnPropertyChanged();
        }
    }

    public Guid Id { get; init; }
    public Guid TargetId { get; init; }
    public string Currency { get; init; }
    public DateTime OccurredAt { get; init; }
}