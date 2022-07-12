using Autofac;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.BuildingBlocks.Infrastructure;
using Budgetter.BuildingBlocks.Infrastructure.AggregateStore;
using Budgetter.BuildingBlocks.Infrastructure.DomainEvents;
using Budgetter.Domain.Targets.Services.Interfaces;
using Budgetter.Infrastructure.AggregateStore;
using Budgetter.Infrastructure.AggregateStore.DataAccess.Ef;
using Budgetter.Infrastructure.AggregateStore.SqlLite;
using Budgetter.Infrastructure.Domain.Ef;
using Budgetter.Infrastructure.Domain.Ef.DataAccessors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Budgetter.Infrastructure.Configuration.DataAccess;

public class DataAccessModule : Module
{
    private readonly EfSettings _efSettings;
    private readonly ILoggerFactory _loggerFactory;

    internal DataAccessModule(
        ILoggerFactory loggerFactory,
        EfSettings efSettings)
    {
        _loggerFactory = loggerFactory;
        _efSettings = efSettings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_efSettings)
            .SingleInstance();

        var streamStore = StreamStoreHelper
            .GetStreamStore(_efSettings);

        if (streamStore != null)
            builder.RegisterInstance(streamStore);

        builder.RegisterType<SqlLiteStreamAggregateStore>()
            .As<IAggregateStore>()
            .InstancePerLifetimeScope();

        builder.Register(context =>
            {
                var sqlSettings = context.Resolve<EfSettings>();
                var aggregateStore = context.Resolve<IAggregateStore>();
                var domainEventsDispatcher = context.Resolve<IDomainEventsDispatcher>();
                var dbContextOptions = new DbContextOptionsBuilder();

                return new BudgetterDbContext(
                    _loggerFactory,
                    sqlSettings,
                    aggregateStore,
                    domainEventsDispatcher,
                    dbContextOptions.Options);
            })
            .AsSelf()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterType<SqlLiteCheckpointStore>()
            .As<ICheckpointStore>()
            .InstancePerLifetimeScope();

        builder.RegisterType(typeof(EfCheckpointAccessor))
            .As<ICheckpointAccessor>()
            .InstancePerLifetimeScope();

        builder.RegisterType<AggregateStoreDomainEventsAccessor>()
            .As<IDomainEventsAccessor>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(Assemblies.ApplicationAssembly)
            .Where(type => type.Name.EndsWith("Projector"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());

        builder.RegisterType<SubscriptionsManager>()
            .As<SubscriptionsManager>()
            .SingleInstance();

        builder.RegisterAssemblyTypes(Assemblies.InfrastructureAssembly)
            .Where(type => type.Name.EndsWith("Accessor"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());

        builder.RegisterType<EfFinanceOperationReadModelAccessor>()
            .As<IFinanceOperationExistenceChecker>()
            .InstancePerLifetimeScope();
    }
}