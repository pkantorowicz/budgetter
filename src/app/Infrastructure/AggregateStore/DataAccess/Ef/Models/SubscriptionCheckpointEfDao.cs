using Budgetter.BuildingBlocks.Infrastructure.AggregateStore;

namespace Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.Models;

public class SubscriptionCheckpointEfDao
{
    public long Position { get; set; }
    public SubscriptionCode Code { get; set; }
}