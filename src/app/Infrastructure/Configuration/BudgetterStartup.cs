using System.Threading.Tasks;
using Autofac;
using Budgetter.Infrastructure.AggregateStore;
using Budgetter.Infrastructure.Configuration.BackgroundJobs;
using Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz;
using Budgetter.Infrastructure.Configuration.DataAccess;
using Budgetter.Infrastructure.Configuration.DomainEvents;
using Budgetter.Infrastructure.Configuration.Execution;
using Budgetter.Infrastructure.Configuration.Feeds;
using Budgetter.Infrastructure.Configuration.Logging;
using Budgetter.Infrastructure.Configuration.Mappings;
using Budgetter.Infrastructure.Configuration.Mediation;
using Budgetter.Infrastructure.Configuration.Notifications;
using Budgetter.Infrastructure.Configuration.Processing;
using Budgetter.Infrastructure.Domain.Ef;
using Budgetter.Infrastructure.Feeds.Exchanges.Nbp;
using Serilog;
using Serilog.Extensions.Logging;

namespace Budgetter.Infrastructure.Configuration;

public class BudgetterStartup
{
    private static IContainer _container;
    private static SubscriptionsManager _subscriptionsManager;

    public static void Initialize(
        EfSettings efSettings,
        NpbApiSettings nbApiSettings,
        ILogger logger,
        bool isDatabaseReinitializationRequired = false)
    {
        logger = logger?.ForContext("App", "Budgetter");

        ConfigureCompositionRoot(
            efSettings,
            nbApiSettings,
            logger);

        InitializeDatabase(isDatabaseReinitializationRequired);
        StartEventProjection();
        StartBackgroundJobProcessing(logger);
    }

    private static void InitializeDatabase(bool isDatabaseReinitializationRequired)
    {
        var dbContext = _container.Resolve<BudgetterDbContext>();

        if (isDatabaseReinitializationRequired)
            dbContext.Database.EnsureDeleted();

        dbContext.Database.EnsureCreated();
    }

    private static void StartEventProjection()
    {
        Task.Run(RunEventsProjectors);
    }

    private static void StartBackgroundJobProcessing(ILogger logger)
    {
        Task.Run(() => QuartzStartup.InitializeAsync(logger)).Wait();
    }

    private static void ConfigureCompositionRoot(
        EfSettings efSettings,
        NpbApiSettings npbApiSettings,
        ILogger logger)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new LoggingModule(logger));
        containerBuilder.RegisterModule(new DomainEventsModule());

        containerBuilder.RegisterModule(new MediationModule(
            Assemblies.GetAssemblies()));

        containerBuilder.RegisterModule(new ProcessingModule());
        containerBuilder.RegisterModule(new MappingsModule());
        containerBuilder.RegisterModule(new FeedsModule(npbApiSettings));
        containerBuilder.RegisterModule(new BackgroundJobsModule());
        containerBuilder.RegisterModule(new NotificationsModule());
        containerBuilder.RegisterModule(new ExecutionModule());

        containerBuilder.RegisterModule(
            new DataAccessModule(
                new SerilogLoggerFactory(logger),
                efSettings));

        _container = containerBuilder.Build();

        BudgetterCompositionRoot.SetContainer(_container);
    }

    private static async Task RunEventsProjectors()
    {
        _subscriptionsManager = _container.Resolve<SubscriptionsManager>();

        await _subscriptionsManager.StartAsync();
    }

    public static void Stop()
    {
        _subscriptionsManager.Stop();
    }
}