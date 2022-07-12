using System;
using System.Globalization;
using System.Windows.Data;

namespace Budgetter.Wpf.Resources.Converters;

internal class InverseBoolPropertyBindingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolProperty)
            return false;

        boolProperty = !boolProperty;

        return boolProperty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}