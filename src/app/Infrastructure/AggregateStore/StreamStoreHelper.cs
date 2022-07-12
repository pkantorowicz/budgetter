using Budgetter.Infrastructure.Domain.Ef;
using SqlStreamStore;
using SqlStreamStore.Infrastructure;

namespace Budgetter.Infrastructure.AggregateStore;

public static class StreamStoreHelper
{
    public static IStreamStore GetStreamStore(EfSettings efSettings)
    {
        return new SqliteStreamStore(
            new SqliteStreamStoreSettings(efSettings.MainConnectionString)
            {
                GetUtcNow = SystemClock.GetUtcNow
            });
    }
}