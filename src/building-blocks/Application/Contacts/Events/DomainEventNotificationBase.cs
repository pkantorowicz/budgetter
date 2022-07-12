using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;

namespace Budgetter.BuildingBlocks.Application.Contacts.Events;

public record DomainNotificationBase<T> : IDomainEventNotification<T>
    where T : IDomainEvent
{
    public DomainNotificationBase(T domainEvent, Guid id)
    {
        Id = id;
        DomainEvent = domainEvent;
    }

    public T DomainEvent { get; }

    public Guid Id { get; }
}