using System.Collections.Generic;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;

namespace Budgetter.Application.BudgetPlans.GetBudgetPlans;

public record GetBudgetPlansQuery : QueryBase<IEnumerable<BudgetPlanDto>>
{
}