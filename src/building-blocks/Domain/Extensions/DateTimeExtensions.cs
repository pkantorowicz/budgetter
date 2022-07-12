using System;
using System.Globalization;

namespace Budgetter.BuildingBlocks.Domain.Extensions;

public static class DateTimeExtensions
{
    public static string ToYmd(this DateTime target)
    {
        return target
            .ToString("u", CultureInfo.InvariantCulture)
            .Split(" ")[0];
    }

    public static string ToYmd(this DateTime? target)
    {
        if (target is null)
            throw new ArgumentNullException(nameof(target), "Target can not be empty.");

        return target.Value
            .ToString("u", CultureInfo.InvariantCulture)
            .Split(" ")[0];
    }
}