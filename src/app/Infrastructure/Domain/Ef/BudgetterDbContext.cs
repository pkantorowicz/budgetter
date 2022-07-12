using System.Threading.Tasks;
using System.Transactions;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.Application.Targets.Projections.ReadModel;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.BuildingBlocks.Infrastructure;
using Budgetter.BuildingBlocks.Infrastructure.DomainEvents;
using Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.EntityTypeConfigurations.SqlLite;
using Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.Models;
using Budgetter.Infrastructure.Domain.Ef.EntityTypeConfigurations.SqlLite;
using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.EntityTypeConfigurations.SqlLite;
using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Budgetter.Infrastructure.Domain.Ef;

public class BudgetterDbContext : DbContext, IUnitOfWork
{
    private readonly IAggregateStore _aggregateStore;
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;
    private readonly EfSettings _efSettings;
    private readonly ILoggerFactory _loggerFactory;

    public BudgetterDbContext(
        ILoggerFactory loggerFactory,
        EfSettings efSettings,
        IAggregateStore aggregateStore,
        IDomainEventsDispatcher domainEventsDispatcher,
        DbContextOptions options)
        : base(options)
    {
        _efSettings = efSettings;
        _aggregateStore = aggregateStore;
        _domainEventsDispatcher = domainEventsDispatcher;
        _loggerFactory = loggerFactory;
    }

    public virtual DbSet<BudgetPlanReadModel> BudgetPlans { get; set; }
    public virtual DbSet<FinanceOperationReadModel> Expenses { get; set; }
    public virtual DbSet<TargetReadModel> Targets { get; set; }
    public virtual DbSet<TargetItemReadModel> TargetItems { get; set; }
    public virtual DbSet<SubscriptionCheckpointEfDao> SubscriptionCheckpoints { get; set; }
    public virtual DbSet<StreamEfDao> Streams { get; set; }
    public virtual DbSet<MessagesEfDao> Messages { get; set; }
    public virtual DbSet<ExchangeRateEfDao> ExchangeRates { get; set; }
    public virtual DbSet<DailyExchangeRatesEfDao> DailyExchangeRates { get; set; }

    public async Task CommitTransactionAsync()
    {
        await _domainEventsDispatcher.DispatchEventsAsync();

        var options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };

        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            options,
            TransactionScopeAsyncFlowOption.Enabled);

        await _aggregateStore.Save();

        transaction.Complete();
    }

    public async Task<bool> SaveAsync()
    {
        var numberOfRows = await SaveChangesAsync();

        return numberOfRows > 0;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        if (builder.IsConfigured)
            return;

        builder
            .UseSqlite(_efSettings.MainConnectionString)
            .UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BudgetPlanSqlLiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FinanceOperationSqlLiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TargetSqlLiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TargetItemSqlLiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionCheckpointSqlLiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StreamsSqlLiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MessagesSqlLiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ExchangeRatesSqlLiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DailyExchangeRatesSqlLiteEntityTypeConfiguration());
    }
}