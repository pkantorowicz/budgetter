using System.Threading.Tasks;

namespace Budgetter.BuildingBlocks.Infrastructure.DomainEvents;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}