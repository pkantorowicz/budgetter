using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.BuildingBlocks.Domain.Types;

namespace Budgetter.BuildingBlocks.Domain.EventSourcing;

/// <summary>
/// </summary>
public interface IAggregateStore
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    Task Save();

    /// <summary>
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> Load<T>(AggregateId<T> aggregateId)
        where T : AggregateRoot;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerable<IDomainEvent> GetChanges();

    /// <summary>
    /// </summary>
    /// <param name="aggregate"></param>
    /// <typeparam name="T"></typeparam>
    void AppendChanges<T>(T aggregate)
        where T : AggregateRoot;

    /// <summary>
    /// </summary>
    void ClearChanges();
}