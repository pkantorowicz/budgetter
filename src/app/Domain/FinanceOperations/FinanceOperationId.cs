using System;
using Budgetter.BuildingBlocks.Domain.Types;

namespace Budgetter.Domain.FinanceOperations;

public class FinanceOperationId : AggregateId<FinanceOperation>
{
    private FinanceOperationId(Guid value) : base(value)
    {
    }

    public static FinanceOperationId New(Guid value)
    {
        return new FinanceOperationId(value);
    }
}