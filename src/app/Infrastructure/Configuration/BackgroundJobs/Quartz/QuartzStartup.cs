using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Serilog;

namespace Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz;

internal class QuartzStartup
{
    public static IScheduler Scheduler;
    public static bool IsInitialized { get; private set; }

    internal static async Task InitializeAsync(ILogger logger)
    {
        logger.Information("Quartz starting...");

        var schedulerConfiguration = new NameValueCollection { { "quartz.scheduler.instanceName", "Budgetter" } };

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);

        Scheduler = await schedulerFactory.GetScheduler();

        LogProvider.SetCurrentLogProvider(new SerilogLogProvider(logger));

        await Scheduler.Start();

        logger.Information("Quartz started");

        IsInitialized = true;
    }

    internal static void StopQuartz()
    {
        Scheduler.Shutdown();
    }
}