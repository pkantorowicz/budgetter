using Autofac;
using Budgetter.BuildingBlocks.Infrastructure.DomainEvents;

namespace Budgetter.Infrastructure.Configuration.DomainEvents;

public class DomainEventsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventsDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();
    }
}