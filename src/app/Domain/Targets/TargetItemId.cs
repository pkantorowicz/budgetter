using System;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.FinanceOperations;

namespace Budgetter.Domain.Targets;

public class TargetItemId : TypedIdBase
{
    private TargetItemId(Guid value) : base(value)
    {
    }

    public static TargetItemId FromExpenseId(FinanceOperationId financeOperationId)
    {
        return new TargetItemId(financeOperationId.Value);
    }

    public static TargetItemId New(Guid id)
    {
        return new TargetItemId(id);
    }
}