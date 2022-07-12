using System;

namespace Budgetter.BuildingBlocks.Domain.Types;

public abstract class AggregateId<T>
    where T : AggregateRoot
{
    protected AggregateId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }
}