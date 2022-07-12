using System.Threading.Tasks;

namespace Budgetter.BuildingBlocks.Infrastructure.AggregateStore;

public interface ICheckpointAccessor
{
    Task<long?> GetCheckpoint(SubscriptionCode subscriptionCode);
    Task InsertCheckpoint(SubscriptionCode subscriptionCheckpointDao, long? checkpoint);
    Task UpdateCheckpoint(SubscriptionCode subscriptionCheckpointDao, long? actualCheckpoint);
}