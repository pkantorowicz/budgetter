using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.BudgetPlans.ChangeBudgetPlanAttributes;
using Budgetter.Application.BudgetPlans.CreateBudgetPlan;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.Application.BudgetPlans.GetBudgetPlan;
using Budgetter.Application.BudgetPlans.GetBudgetPlans;
using Budgetter.Application.BudgetPlans.RemoveBudgetPlan;
using Budgetter.Wpf.Controllers.Base;
using Budgetter.Wpf.Controllers.Interfaces;

namespace Budgetter.Wpf.Controllers;

internal class BudgetPlanController : ControllerBase, IBudgetPlanController
{
    public async Task<BudgetPlanDto> GetBudgetPlan(Guid budgetPlanId)
    {
        var budgetPlan = await QueryAsync(
            new GetBudgetPlanQuery(
                budgetPlanId));

        return budgetPlan;
    }

    public async Task<IEnumerable<BudgetPlanDto>> GetBudgetPlans()
    {
        var budgetPlans = await QueryAsync(
            new GetBudgetPlansQuery());

        return budgetPlans;
    }

    public async Task<Guid> CreateBudgetPlan(
        string title,
        string currency,
        DateTime validFrom,
        DateTime validTo,
        int maxDaysCount)
    {
        var budgetPlanId = await SendAsync(
            new CreateBudgetPlanCommand(
                title,
                currency,
                validFrom,
                validTo,
                maxDaysCount));

        return budgetPlanId;
    }

    public async Task ChangeBudgetPlanAttributes(
        Guid budgetPlanId,
        string title,
        string currency,
        DateTime validFrom,
        DateTime validTo,
        int maxDaysCount)
    {
        await SendAsync(
            new ChangeBudgetPlanAttributesCommand(
                budgetPlanId,
                title,
                currency,
                validFrom,
                validTo,
                maxDaysCount));
    }

    public async Task Remove(Guid budgetPlanId)
    {
        await SendAsync(new RemoveBudgetPlanCommand(budgetPlanId));
    }
}