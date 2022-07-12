using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Budgetter.Wpf.Framework.Extensions;
using Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

namespace Budgetter.Wpf.Resources.Converters;

internal class FinanceOperationTypeToForegroundColorConverter : IValueConverter
{
    public const string LightForegroundResourceName = "MaterialDesignLightForeground";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var solidColorBrush = value switch
        {
            FinanceOperationType.Income => new SolidColorBrush(Colors.ForestGreen),
            FinanceOperationType.Expense => new SolidColorBrush(Colors.Red),
            _ => ResourceDictionaryExtensions.GetResource<SolidColorBrush>(LightForegroundResourceName)
        };

        return solidColorBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}