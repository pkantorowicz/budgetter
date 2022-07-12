using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Infrastructure.AggregateStore;

namespace Budgetter.Infrastructure.AggregateStore.SqlLite;

public class SqlLiteCheckpointStore : ICheckpointStore
{
    private readonly ICheckpointAccessor _checkpointAccessor;

    public SqlLiteCheckpointStore(ICheckpointAccessor checkpointAccessor)
    {
        _checkpointAccessor = checkpointAccessor;
    }

    public async Task<long?> GetCheckpoint(SubscriptionCode subscriptionCode)
    {
        return await _checkpointAccessor.GetCheckpoint(subscriptionCode);
    }

    public async Task StoreCheckpoint(SubscriptionCode subscriptionCode, long checkpoint)
    {
        var actualCheckpoint = await GetCheckpoint(subscriptionCode);

        if (actualCheckpoint == null)
            await _checkpointAccessor.InsertCheckpoint(
                subscriptionCode,
                checkpoint);
        else
            await _checkpointAccessor
                .UpdateCheckpoint(
                    subscriptionCode,
                    checkpoint);
    }
}