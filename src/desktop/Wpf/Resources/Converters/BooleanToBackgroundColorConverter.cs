using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Budgetter.Wpf.Resources.Converters;

internal class BooleanToBackgroundColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool isSuccess)
            return default;

        var solidColorBrush = isSuccess
            ? new SolidColorBrush(Colors.ForestGreen)
            : new SolidColorBrush(Colors.Red);

        solidColorBrush.Opacity = 0.5;

        return solidColorBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}