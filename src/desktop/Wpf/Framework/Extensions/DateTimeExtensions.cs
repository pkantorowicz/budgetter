using System;
using System.Globalization;

namespace Budgetter.Wpf.Framework.Extensions;

internal static class DateTimeExtensions
{
    public static bool CheckIfTextIsCorrectDateTime(this string text, string format = null)
    {
        return DateTime.TryParseExact(
            text,
            format ?? "dd.mm.yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None, out _);
    }

    public static Tuple<DateTime, DateTime> GetMonthStartAndEndDates(this DateTime currentDate)
    {
        return new Tuple<DateTime, DateTime>(
            currentDate.ModifyDate(day: 1),
            currentDate.ModifyDate(day: DateTime.DaysInMonth(currentDate.Year, currentDate.Month)));
    }

    public static Tuple<DateTime, DateTime> GetQuarterStartAndEndDates(this DateTime currentDate)
    {
        var quarter = GetQuarter(currentDate);

        return quarter switch
        {
            1 => GetStartAndEndOfQuarter(currentDate, 1),
            2 => GetStartAndEndOfQuarter(currentDate, 4),
            3 => GetStartAndEndOfQuarter(currentDate, 7),
            4 => GetStartAndEndOfQuarter(currentDate, 10),
            _ => throw new ArgumentOutOfRangeException(
                nameof(quarter),
                quarter,
                "Unknown quarter.")
        };
    }

    public static bool CheckMonthDates(this DateTime currentDate, DateTime? validFrom, DateTime? validTo)
    {
        var (monthStartDate, monthEndDate) = currentDate.GetMonthStartAndEndDates();

        if (!validFrom.HasValue || !validTo.HasValue)
            return false;

        return monthStartDate.Date == validFrom.Value.Date && monthEndDate.Date == validTo.Value.Date;
    }

    public static bool CheckQuarterDates(this DateTime currentDate, DateTime? validFrom, DateTime? validTo)
    {
        var (quarterStartDate, quarterEndDate) = currentDate.GetQuarterStartAndEndDates();

        if (!validFrom.HasValue || !validTo.HasValue)
            return false;

        return quarterStartDate.Date == validFrom.Value.Date && quarterEndDate.Date == validTo.Value.Date;
    }

    public static int GetQuarter(this DateTime currentDate)
    {
        return decimal.ToInt32(Math.Ceiling(currentDate.Month / 3.0m));
    }

    private static DateTime ModifyDate(
        this DateTime currentDate,
        int? year = null,
        int? month = null,
        int? day = null)
    {
        return new DateTime(
            year ?? currentDate.Year,
            month ?? currentDate.Month,
            day ?? currentDate.Day,
            currentDate.Hour,
            currentDate.Minute,
            currentDate.Second,
            currentDate.Millisecond);
    }

    private static Tuple<DateTime, DateTime> GetStartAndEndOfQuarter(
        DateTime currentDate,
        int month)
    {
        return new Tuple<DateTime, DateTime>(currentDate.ModifyDate(month: month, day: 1),
            currentDate.ModifyDate(month: month + 2,
                day: DateTime.DaysInMonth(currentDate.Year, currentDate.Month)));
    }
}