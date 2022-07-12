using System.Threading.Tasks;

namespace Budgetter.BuildingBlocks.Infrastructure.AggregateStore;

public interface ICheckpointStore
{
    Task<long?> GetCheckpoint(SubscriptionCode subscriptionCode);

    Task StoreCheckpoint(SubscriptionCode subscriptionCode, long checkpoint);
}