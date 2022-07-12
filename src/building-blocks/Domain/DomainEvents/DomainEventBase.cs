using System;

namespace Budgetter.BuildingBlocks.Domain.DomainEvents;

public record DomainEventBase : IDomainEvent
{
    protected DomainEventBase()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}