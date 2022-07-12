using System;
using System.Windows;

namespace Budgetter.Wpf.Views.Helpers;

internal static class ScreenDimensionsHelper
{
    public static Tuple<double, double> GetScreenDimensions(
        double? height = null,
        double? width = null,
        double percents = 1.0)
    {
        var screenHeight = percents * height ?? SystemParameters.PrimaryScreenHeight;
        var screenWidth = percents * width ?? SystemParameters.PrimaryScreenWidth;

        return new Tuple<double, double>(screenHeight, screenWidth);
    }
}