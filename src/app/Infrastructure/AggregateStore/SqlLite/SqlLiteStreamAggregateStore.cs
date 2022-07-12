using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.BuildingBlocks.Infrastructure.Serialization;
using Budgetter.Infrastructure.Domain.Ef;
using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Streams;

namespace Budgetter.Infrastructure.AggregateStore.SqlLite;

public class SqlLiteStreamAggregateStore : IAggregateStore
{
    private readonly List<AggregateToSave> _aggregatesToSave;
    private readonly List<IDomainEvent> _appendedChanges;
    private readonly IStreamStore _streamStore;

    public SqlLiteStreamAggregateStore(EfSettings efSettings)
    {
        _streamStore = StreamStoreHelper
            .GetStreamStore(efSettings);

        _appendedChanges = new List<IDomainEvent>();
        _aggregatesToSave = new List<AggregateToSave>();
    }

    public async Task Save()
    {
        foreach (var aggregateToSave in _aggregatesToSave)
            await _streamStore.AppendToStream(
                GetStreamId(aggregateToSave.Aggregate),
                aggregateToSave.Aggregate.Version,
                aggregateToSave.Messages.ToArray());

        _aggregatesToSave.Clear();
    }

    public async Task<T> Load<T>(AggregateId<T> aggregateId)
        where T : AggregateRoot
    {
        var streamId = GetStreamId(aggregateId);

        IList<IDomainEvent> domainEvents = new List<IDomainEvent>();
        ReadStreamPage readStreamPage;
        var position = StreamVersion.Start;
        var take = 100;
        do
        {
            readStreamPage = await _streamStore.ReadStreamForwards(streamId, position, take);
            var messages = readStreamPage.Messages;

            foreach (var streamMessage in messages)
            {
                var type = DomainEventTypeMappings.Dictionary[streamMessage.Type];
                var jsonData = await streamMessage.GetJsonData();
                var domainEvent = JsonConvert.DeserializeObject(jsonData, type) as IDomainEvent;

                domainEvents.Add(domainEvent);
            }

            position += take;
        } while (!readStreamPage.IsEnd);

        if (!domainEvents.Any()) return null;

        var aggregate = (T)Activator.CreateInstance(typeof(T), true);

        aggregate?.Load(domainEvents);

        return aggregate;
    }

    public IEnumerable<IDomainEvent> GetChanges()
    {
        return _appendedChanges;
    }

    public void AppendChanges<T>(T aggregate)
        where T : AggregateRoot
    {
        _aggregatesToSave.Add(new AggregateToSave(aggregate, CreateStreamMessages(aggregate).ToList()));
    }

    public void ClearChanges()
    {
        _appendedChanges.Clear();
    }

    private IEnumerable<NewStreamMessage> CreateStreamMessages<T>(
        T aggregate)
        where T : AggregateRoot
    {
        var newStreamMessages = new List<NewStreamMessage>();
        var domainEvents = aggregate.GetDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            var jsonData = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver()
            });

            var message = new NewStreamMessage(
                domainEvent.EventId,
                MapDomainEventToType(domainEvent),
                jsonData);

            newStreamMessages.Add(message);
            _appendedChanges.Add(domainEvent);
        }

        return newStreamMessages.ToArray();
    }

    private static string MapDomainEventToType(IDomainEvent domainEvent)
    {
        foreach (var key in DomainEventTypeMappings.Dictionary.Keys)
            if (DomainEventTypeMappings.Dictionary[key] == domainEvent.GetType())
                return key;

        throw new ArgumentException("Invalid Domain Event type", nameof(domainEvent));
    }

    private static string GetStreamId<T>(T aggregate)
        where T : AggregateRoot
    {
        return $"{aggregate.GetType().Name}-{aggregate.Id:N}";
    }

    private static string GetStreamId<T>(AggregateId<T> aggregateId)
        where T : AggregateRoot
    {
        return $"{typeof(T).Name}-{aggregateId.Value:N}";
    }

    private class AggregateToSave
    {
        public AggregateToSave(AggregateRoot aggregate, List<NewStreamMessage> messages)
        {
            Aggregate = aggregate;
            Messages = messages;
        }

        public AggregateRoot Aggregate { get; }

        public List<NewStreamMessage> Messages { get; }
    }
}