using System;
using System.Threading.Tasks;

namespace Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Jobs.DownloadExchangeRates;

public interface IDownloadExchangeRatesJobManager
{
    Task TriggerDownloadExchangeRatesJob(
        DateTime from,
        DateTime to,
        int maxDaysCount);

    Task ScheduleDownloadExchangeRatesJob(
        DateTime from,
        DateTime to,
        int maxDaysCount);

    Task PauseDownloadExchangeRatesJob();

    Task ResumeDownloadExchangeRatesJob();
}