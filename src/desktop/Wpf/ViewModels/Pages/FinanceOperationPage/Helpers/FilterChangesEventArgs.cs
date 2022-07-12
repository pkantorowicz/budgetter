using System;

namespace Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

internal class FilterChangesEventArgs : EventArgs
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}