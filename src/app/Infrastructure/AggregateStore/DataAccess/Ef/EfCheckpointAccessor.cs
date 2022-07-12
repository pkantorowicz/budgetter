using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Infrastructure.AggregateStore;
using Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.Models;
using Budgetter.Infrastructure.Domain.Ef;
using Microsoft.EntityFrameworkCore;

namespace Budgetter.Infrastructure.AggregateStore.DataAccess.Ef;

public class EfCheckpointAccessor : ICheckpointAccessor
{
    private readonly BudgetterDbContext _budgetterDbContext;
    private readonly DbSet<SubscriptionCheckpointEfDao> _subscriptionCheckpointsDbSet;

    public EfCheckpointAccessor(BudgetterDbContext budgetterDbContext)
    {
        _budgetterDbContext = budgetterDbContext;

        _subscriptionCheckpointsDbSet = _budgetterDbContext.Set<SubscriptionCheckpointEfDao>();
    }

    public async Task<long?> GetCheckpoint(SubscriptionCode subscriptionCode)
    {
        var checkpoint = await GetCurrentCheckpoint(subscriptionCode);

        return checkpoint?.Position;
    }

    public async Task InsertCheckpoint(SubscriptionCode subscriptionCheckpointDao, long? checkpoint)
    {
        await _subscriptionCheckpointsDbSet.AddAsync(
            new SubscriptionCheckpointEfDao
            {
                Code = subscriptionCheckpointDao,
                Position = checkpoint ?? 0
            });

        await _budgetterDbContext.SaveAsync();
    }

    public async Task UpdateCheckpoint(SubscriptionCode subscriptionCode, long? actualCheckpoint)
    {
        var checkpoint = await GetCurrentCheckpoint(subscriptionCode);

        if (actualCheckpoint.HasValue)
        {
            checkpoint.Position = actualCheckpoint.Value;
            _budgetterDbContext.Update(checkpoint);
            await _budgetterDbContext.SaveAsync();
        }
    }

    private async Task<SubscriptionCheckpointEfDao> GetCurrentCheckpoint(SubscriptionCode subscriptionCode)
    {
        return await _subscriptionCheckpointsDbSet
            .FirstOrDefaultAsync(sc => sc.Code == subscriptionCode);
    }
}