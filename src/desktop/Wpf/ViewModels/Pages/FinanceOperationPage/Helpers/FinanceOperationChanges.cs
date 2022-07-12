using System;

namespace Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

internal record FinanceOperationChanges
{
    public FinanceOperationChanges(
        string title,
        decimal price,
        DateTime occurredAt)
    {
        Title = title;
        Price = price;
        OccurredAt = occurredAt;
    }

    public string Title { get; }
    public decimal Price { get; }
    public DateTime OccurredAt { get; }
}