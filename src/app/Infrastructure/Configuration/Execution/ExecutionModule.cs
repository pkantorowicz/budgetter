using Autofac;
using Budgetter.BuildingBlocks.Application.Execution;

namespace Budgetter.Infrastructure.Configuration.Execution;

public class ExecutionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Executor>()
            .As<IExecutor>()
            .InstancePerLifetimeScope();
    }
}