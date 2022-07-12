using Autofac;
using Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz;

namespace Budgetter.Infrastructure.Configuration.BackgroundJobs;

public class BackgroundJobsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(new QuartzStartup())
            .SingleInstance();

        builder.RegisterModule<QuartzModule>();
    }
}