using System.Threading.Tasks;
using MediatR;

namespace Budgetter.BuildingBlocks.Infrastructure.DomainEvents;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IDomainEventsAccessor _domainEventsProvider;
    private readonly IMediator _mediator;

    public DomainEventsDispatcher(
        IMediator mediator,
        IDomainEventsAccessor domainEventsProvider)
    {
        _mediator = mediator;
        _domainEventsProvider = domainEventsProvider;
    }

    public async Task DispatchEventsAsync()
    {
        var domainEvents = _domainEventsProvider.GetAllDomainEvents();

        _domainEventsProvider.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }
}