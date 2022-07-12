using System;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;

namespace Budgetter.Application.BudgetPlans.GetBudgetPlan;

public record GetBudgetPlanQuery : QueryBase<BudgetPlanDto>
{
    public GetBudgetPlanQuery(Guid budgetPlanId)
    {
        BudgetPlanId = budgetPlanId;
    }

    public Guid BudgetPlanId { get; }
}