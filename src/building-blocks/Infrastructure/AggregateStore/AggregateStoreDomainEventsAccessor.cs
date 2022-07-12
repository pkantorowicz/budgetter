using System.Collections.Generic;
using System.Linq;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.BuildingBlocks.Infrastructure.DomainEvents;

namespace Budgetter.BuildingBlocks.Infrastructure.AggregateStore;

public class AggregateStoreDomainEventsAccessor : IDomainEventsAccessor
{
    private readonly IAggregateStore _aggregateStore;

    public AggregateStoreDomainEventsAccessor(IAggregateStore aggregateStore)
    {
        _aggregateStore = aggregateStore;
    }

    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
    {
        return _aggregateStore
            .GetChanges()
            .ToList()
            .AsReadOnly();
    }

    public void ClearAllDomainEvents()
    {
        _aggregateStore.ClearChanges();
    }
}