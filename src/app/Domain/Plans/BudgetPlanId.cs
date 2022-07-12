using System;
using Budgetter.BuildingBlocks.Domain.Types;

namespace Budgetter.Domain.Plans;

public class BudgetPlanId : AggregateId<BudgetPlan>
{
    private BudgetPlanId(Guid value) : base(value)
    {
    }

    public static BudgetPlanId New(Guid budgetPlanId)
    {
        return new BudgetPlanId(budgetPlanId);
    }
}