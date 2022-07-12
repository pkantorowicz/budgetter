using System;
using System.Collections.Generic;
using Budgetter.BuildingBlocks.Domain.DomainEvents;

namespace Budgetter.BuildingBlocks.Domain.Types;

public abstract class AggregateRoot : IAggregateRoot, IPersistDomainObject
{
    private readonly List<IDomainEvent> _domainEvents;

    protected AggregateRoot()
    {
        _domainEvents = new List<IDomainEvent>();

        Version = -1;
    }

    public int Version { get; private set; }
    public Guid Id { get; protected set; }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    protected void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void Load(IEnumerable<IDomainEvent> history)
    {
        foreach (var e in history)
        {
            Apply(e);
            Version++;
        }
    }

    protected abstract void Apply(IDomainEvent @event);
}