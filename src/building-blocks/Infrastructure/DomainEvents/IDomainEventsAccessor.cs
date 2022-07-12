using System.Collections.Generic;
using Budgetter.BuildingBlocks.Domain.DomainEvents;

namespace Budgetter.BuildingBlocks.Infrastructure.DomainEvents;

public interface IDomainEventsAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}