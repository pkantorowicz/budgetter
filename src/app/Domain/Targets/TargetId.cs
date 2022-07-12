using System;
using Budgetter.BuildingBlocks.Domain.Types;

namespace Budgetter.Domain.Targets;

public class TargetId : AggregateId<Target>
{
    private TargetId(Guid value) : base(value)
    {
    }

    public static TargetId New(Guid value)
    {
        return new TargetId(value);
    }
}