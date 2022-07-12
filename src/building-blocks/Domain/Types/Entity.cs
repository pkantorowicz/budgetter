using System;
using System.Collections.Generic;
using Budgetter.BuildingBlocks.Domain.DomainEvents;

namespace Budgetter.BuildingBlocks.Domain.Types;

public abstract class Entity : IEntity
{
    private List<IDomainEvent> _domainEvents;

    public Guid Id { get; protected init; }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();

        _domainEvents.Add(domainEvent);
    }
}