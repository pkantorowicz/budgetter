using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Budgetter.BuildingBlocks.Application.Projections;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.BuildingBlocks.Infrastructure.AggregateStore;
using Budgetter.Infrastructure.Configuration;
using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Streams;

namespace Budgetter.Infrastructure.AggregateStore;

public class SubscriptionsManager
{
    private readonly IStreamStore _streamStore;

    public SubscriptionsManager(
        IStreamStore streamStore)
    {
        _streamStore = streamStore;
    }

    public async Task StartAsync()
    {
        long? actualPosition;

        await using (var scope = BudgetterCompositionRoot.BeginLifetimeScope())
        {
            var checkpointStore = scope.Resolve<ICheckpointStore>();

            actualPosition = await checkpointStore.GetCheckpoint(SubscriptionCode.All);
        }

        _streamStore.SubscribeToAll(actualPosition, StreamMessageReceived);
    }

    public void Stop()
    {
        _streamStore.Dispose();
    }

    private static async Task StreamMessageReceived(
        IAllStreamSubscription subscription,
        StreamMessage streamMessage,
        CancellationToken cancellationToken)
    {
        var type = DomainEventTypeMappings.Dictionary[streamMessage.Type];
        var jsonData = await streamMessage.GetJsonData(cancellationToken);
        var domainEvent = JsonConvert.DeserializeObject(jsonData, type) as IDomainEvent;

        await using var scope = BudgetterCompositionRoot.BeginLifetimeScope();

        var projectors = scope.Resolve<IList<IProjector>>();

        foreach (var projector in projectors)
            await projector.Project(domainEvent);

        var checkpointStore = scope.Resolve<ICheckpointStore>();

        await checkpointStore.StoreCheckpoint(
            SubscriptionCode.All, streamMessage.Position);
    }
}