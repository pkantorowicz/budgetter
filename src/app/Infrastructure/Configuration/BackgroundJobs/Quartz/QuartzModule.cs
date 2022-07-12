using Autofac;
using Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Jobs.DownloadExchangeRates;
using Quartz;

namespace Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz;

public class QuartzModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DownloadExchangeRatesJobManager>()
            .As<IDownloadExchangeRatesJobManager>()
            .SingleInstance();

        builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(x => typeof(IJob).IsAssignableFrom(x))
            .InstancePerDependency();
    }
}