namespace Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

internal enum FilterChangeType
{
    None,
    StartDate,
    EndDate,
    MinPrice,
    MaxPrice,
    CurrentMonth,
    LastMonth,
    LastThreeMonths,
    LastHalfYear
}