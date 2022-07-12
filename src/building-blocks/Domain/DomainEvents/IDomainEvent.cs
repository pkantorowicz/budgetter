using System;

namespace Budgetter.BuildingBlocks.Domain.DomainEvents;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}