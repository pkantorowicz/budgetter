using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Domain.DomainEvents;

namespace Budgetter.BuildingBlocks.Application.Projections;

public interface IProjector
{
    Task Project(IDomainEvent @event);
}