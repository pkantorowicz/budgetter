namespace Budgetter.Wpf.Framework.Extensions;

internal static class ConversionExtensions
{
    public static decimal ConvertToDecimalOrDefault(this string value)
    {
        try
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            var splitValues = value.Split('.');
            var trimmedValue = splitValues[0];

            return decimal.Parse(trimmedValue);
        }
        catch
        {
            return 0;
        }
    }
}