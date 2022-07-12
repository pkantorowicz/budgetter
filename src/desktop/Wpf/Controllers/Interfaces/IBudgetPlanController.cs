using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.BudgetPlans.Dtos;

namespace Budgetter.Wpf.Controllers.Interfaces;

internal interface IBudgetPlanController
{
    Task<BudgetPlanDto> GetBudgetPlan(Guid budgetPlanId);
    Task<IEnumerable<BudgetPlanDto>> GetBudgetPlans();

    Task<Guid> CreateBudgetPlan(
        string title,
        string currency,
        DateTime validFrom,
        DateTime validTo,
        int maxDaysCount);

    Task ChangeBudgetPlanAttributes(
        Guid budgetPlanId,
        string title,
        string currency,
        DateTime validFrom,
        DateTime validTo,
        int maxDaysCount);

    Task Remove(Guid budgetPlanId);
}