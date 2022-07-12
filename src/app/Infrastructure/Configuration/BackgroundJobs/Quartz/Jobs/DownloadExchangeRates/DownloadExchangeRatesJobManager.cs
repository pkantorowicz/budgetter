using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Domain.Extensions;
using Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Exceptions;
using Quartz;

namespace Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Jobs.DownloadExchangeRates;

internal class DownloadExchangeRatesJobManager : IDownloadExchangeRatesJobManager
{
    private const string RatesDownloadJobIdentity = "DownloadExchangeRatesJob";

    private static IScheduler Scheduler => QuartzStartup.IsInitialized
        ? QuartzStartup.Scheduler
        : throw new QuartzNotInitializedException(
            "Quartz is not initialized yet.");

    public async Task TriggerDownloadExchangeRatesJob(
        DateTime from,
        DateTime to,
        int maxDaysCount)
    {
        var jobData = CreateJobData(
            from,
            to,
            maxDaysCount);

        var downloadExchangeRatesJob = JobBuilder
            .Create<DownloadExchangeRatesJob>()
            .WithIdentity(new JobKey(RatesDownloadJobIdentity))
            .StoreDurably()
            .Build();


        await Scheduler.AddJob(downloadExchangeRatesJob, true);
        await Scheduler.TriggerJob(new JobKey(RatesDownloadJobIdentity), jobData);
    }

    public async Task ScheduleDownloadExchangeRatesJob(
        DateTime from,
        DateTime to,
        int maxDaysCount)
    {
        var jobData = CreateJobData(
            from,
            to,
            maxDaysCount);

        var downloadExchangeRatesJob = JobBuilder
            .Create<DownloadExchangeRatesJob>()
            .WithIdentity(new JobKey(RatesDownloadJobIdentity))
            .Build();

        var triggerCommandsProcessing =
            TriggerBuilder
                .Create()
                .StartNow()
                .UsingJobData(jobData)
                .WithCronSchedule("0 0/5 * * * ?")
                .Build();

        await Scheduler.ScheduleJob(downloadExchangeRatesJob, triggerCommandsProcessing);
    }

    public async Task PauseDownloadExchangeRatesJob()
    {
        await Scheduler.PauseJob(new JobKey(RatesDownloadJobIdentity));
    }

    public async Task ResumeDownloadExchangeRatesJob()
    {
        await Scheduler.ResumeJob(new JobKey(RatesDownloadJobIdentity));
    }

    private static JobDataMap CreateJobData(
        DateTime from,
        DateTime to,
        int maxDaysCount)
    {
        var jobParamsDictionary = new Dictionary<string, string>
        {
            { "from", from.ToYmd() },
            { "to", to.ToYmd() },
            { "maxDaysCount", maxDaysCount.ToString() }
        };

        var jobData = new JobDataMap(jobParamsDictionary);

        return jobData;
    }
}