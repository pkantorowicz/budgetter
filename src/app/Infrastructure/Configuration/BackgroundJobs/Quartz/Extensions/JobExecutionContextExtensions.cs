using System;
using Quartz;

namespace Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Extensions;

public static class JobExecutionContextExtensions
{
    public static DateTime GetDateTimeValueFromJobContext(this IJobExecutionContext context, string key)
    {
        if (context.MergedJobDataMap[key] is not string fromStringValue)
            return default;

        var isSuccessfullyParsed = DateTime.TryParse(fromStringValue, out var dateTimeValue);

        return isSuccessfullyParsed ? dateTimeValue : default;
    }

    public static int GetIntValueFromJobContext(this IJobExecutionContext context, string key)
    {
        if (context.MergedJobDataMap[key] is not string fromStringValue)
            return default;

        var isSuccessfullyParsed = int.TryParse(fromStringValue, out var intValue);

        return isSuccessfullyParsed ? intValue : default;
    }
}